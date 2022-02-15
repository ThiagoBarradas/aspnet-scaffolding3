using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AspNetScaffolding3.Extensions.RequestLimit
{
    public class CustomRateLimitConfiguration : RateLimitConfiguration
    {
        public CustomRateLimitConfiguration(
            IHttpContextAccessor httpContextAccessor,
            IOptions<IpRateLimitOptions> ipOptions,
            IOptions<ClientRateLimitOptions> clientOptions) : base(httpContextAccessor, ipOptions, clientOptions)
        {
        }

        protected override void RegisterResolvers()
        {
            base.RegisterResolvers();

            ClientResolvers.Add(new UrlResourceRateLimitContributor(HttpContextAccessor));
        }
    }
}
