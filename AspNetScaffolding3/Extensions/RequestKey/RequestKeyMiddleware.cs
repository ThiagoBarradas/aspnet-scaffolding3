using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.RequestKey
{
    public class RequestKeyMiddleware : IMiddleware
    {
        private RequestKey RequestKey { get; set; }

        public RequestKeyMiddleware(RequestKey requestKey)
        {
            RequestKey = requestKey;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Request.EnableBuffering();
            }
            catch (Exception) { }

            if (context.Request.Headers.ContainsKey(RequestKeyServiceExtension.RequestKeyHeaderName))
            {
                RequestKey.Value = context.Request.Headers[RequestKeyServiceExtension.RequestKeyHeaderName];
            }
            else
            {
                RequestKey.Value = Guid.NewGuid().ToString();
            }

            context.Items.Add(RequestKeyServiceExtension.RequestKeyHeaderName, RequestKey.Value);
            context.Response.Headers.Add(RequestKeyServiceExtension.RequestKeyHeaderName, RequestKey.Value);

            await next(context);
        }
    }

    public static class RequestKeyMiddlewareExtension
    {
        public static void UseRequestKey(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestKeyMiddleware>();
        }
    }
}
