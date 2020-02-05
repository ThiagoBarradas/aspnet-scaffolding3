using AspNetScaffolding.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Healthcheck
{
    public static class HealthcheckkMiddlewareExtension
    {
        public static void UseHealthcheck(this IApplicationBuilder app)
        {
            if (HealthcheckServiceExtension.HealthcheckSettings?.Enabled == true)
            {
                var options = new HealthCheckOptions
                {
                    AllowCachingResponses = true,
                    ResponseWriter = WriteResponse
                };

                app.UseHealthChecks(
                    GetFullPath(HealthcheckServiceExtension.ApiSettings, HealthcheckServiceExtension.HealthcheckSettings), options);
            }
        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString().ToLowerInvariant()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString().ToLowerInvariant()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }

        public static string GetFullPath(ApiSettings apiSettings, HealthcheckSettings healthcheckSettings)
        {
            var basePath = apiSettings.GetPathPrefixConsideringVersion();
            basePath = ((string.IsNullOrWhiteSpace(basePath) == false) ? "/" + basePath.Trim('/') : "");
            var finalPathPart = healthcheckSettings.Path?.Trim('/');

            return (basePath ?? "") + "/" + (finalPathPart ?? "healthcheck");
        }
    }
}

