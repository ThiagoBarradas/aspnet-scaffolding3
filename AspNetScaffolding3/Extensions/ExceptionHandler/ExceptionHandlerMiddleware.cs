using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Models;
using AspNetScaffolding.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using WebApi.Models.Exceptions;
using WebApi.Models.Helpers;

namespace AspNetScaffolding.Extensions.ExceptionHandler
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly bool IsDevelopment;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            Next = next;

            IsDevelopment = EnvironmentUtility.IsDevelopment();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, IsDevelopment);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool isDevelopment)
        {
            try
            {
                context.Request.Body.Position = 0;
            }
            catch { }

            if (exception is ApiException)
            {
                return ApiException(context, (ApiException)exception);
            }
            else
            {
                return GenericError(context, exception, isDevelopment);
            }
        }

        private static Task GenericError(HttpContext context, Exception exception, bool isDevelopment)
        {
            context.Items.Add("Exception", exception);

            if (isDevelopment)
            {
                var exceptionContainer = new ExceptionContainer(exception);
                context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionContainer, JsonSerializerService.JsonSerializerSettings)).Wait();
                context.Response.Body.Position = 0;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return Task.CompletedTask;
        }

        private static Task ApiException(HttpContext context, ApiException exception)
        {
            var apiResponse = exception.ToApiResponse();
            var statusCode = (int)apiResponse.StatusCode;

            if (exception is PermanentRedirectException)
            {
                statusCode = 308;
                var location = $"{Api.ApiSettings.AppUrl.Trim('/')}{context.Request.Path}{context.Request.QueryString}";

                context.Response.Headers["Location"] = location;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            if (apiResponse.Content != null)
            {
                context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse.Content, JsonSerializerService.JsonSerializerSettings)).Wait();
                context.Response.Body.Position = 0;
            }

            return Task.CompletedTask;
        }
    }

    public static class ExceptionHandlerMiddlewareExtension
    {
        public static void UseScaffoldingExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
