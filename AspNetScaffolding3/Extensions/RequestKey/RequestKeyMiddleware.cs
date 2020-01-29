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
            this.RequestKey = requestKey;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.ContainsKey(RequestKeyServiceExtension.RequestKeyHeaderName))
            {
                this.RequestKey.Value = context.Request.Headers[RequestKeyServiceExtension.RequestKeyHeaderName];
            }
            else
            {
                this.RequestKey.Value = Guid.NewGuid().ToString();
            }

            context.Items.Add(RequestKeyServiceExtension.RequestKeyHeaderName, this.RequestKey.Value);
            context.Response.Headers.Add(RequestKeyServiceExtension.RequestKeyHeaderName, this.RequestKey.Value);

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
