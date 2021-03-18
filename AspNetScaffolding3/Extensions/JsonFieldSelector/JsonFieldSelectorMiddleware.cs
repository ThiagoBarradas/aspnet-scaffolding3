using AspNetScaffolding.Extensions.StreamExt;
using JsonFieldSelector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.JsonFieldSelector
{
    public class JsonFieldSelectorMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly string PropertyName;

        public JsonFieldSelectorMiddleware(RequestDelegate next)
        {
            this.Next = next;
            this.PropertyName = "fields";
        }

        public JsonFieldSelectorMiddleware(RequestDelegate next, string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.Next = next;
            this.PropertyName = property.ToLowerInvariant();
        }

        public async Task Invoke(HttpContext context)
        {
            await this.Next(context);

            if (context.Request.Query.Any(r => r.Key.ToLowerInvariant() == this.PropertyName))
            {
                var json = context.Response.Body.AsString();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }

                try
                {
                    var fields = context.Request.Query
                        .FirstOrDefault(r => r.Key.ToLowerInvariant() == this.PropertyName)
                        .Value.ToString();

                    if (string.IsNullOrWhiteSpace(fields))
                    {
                        json = "{ }";
                    }
                    else
                    {
                        json = JsonFieldSelectorExtension.SelectFieldsFromString(json, fields);
                    }
                }
                catch
                {
                    json = "{ }";
                }

                var length = Encoding.UTF8.GetBytes(json ?? "").Length;
                context.Response.Body.SetLength(length);
                context.Response.ContentLength = length;
                await context.Response.WriteAsync(json);
            }
        }
    }
}

namespace AspNetScaffolding.Extensions.JsonFieldSelector
{
    public static class JsonFieldSelectorMiddlewareExtension
    {
        public static void UseJsonFieldSelector(this IApplicationBuilder app, string property = "fields", bool enabled = true)
        {
            if (enabled)
            {
                app.UseMiddleware<JsonFieldSelectorMiddleware>(property);
            }
        }
    }
}
