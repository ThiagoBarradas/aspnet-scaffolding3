using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetScaffolding3.Extensions.RequestLimit
{
    public class UrlResourceRateLimitContributor : IClientResolveContributor
    {
        public static string UrlResource;

        private IHttpContextAccessor httpContextAccessor;

        public UrlResourceRateLimitContributor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string ResolveClient()
        {
            string clientId = null;

            string[] urlSections = this.httpContextAccessor.HttpContext.Request.Path.Value?.Split('/');

            for (int urlSectionPosition = 0; urlSectionPosition < urlSections.Length && clientId == null; urlSectionPosition++)
            {
                if (urlSections[urlSectionPosition] == UrlResource)
                {
                    clientId = urlSections[urlSectionPosition + 1];
                }
            }

            return clientId;
        }
    }
}