using AspNetCoreRateLimit;
using AspNetScaffolding.Extensions.Cache;
using AspNetScaffolding.Extensions.RequestLimit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetScaffolding.Extensions.RequestLimit
{
    public static class IpRateLimitingService
    {
        public static void SetupIpRateLimiting(
            this IServiceCollection services,
            IpRateLimitingAdditional ipRateLimitingAdditional,
            CacheSettings cacheSettings)
        {
            if (ipRateLimitingAdditional?.Enabled == true)
            {
                if (cacheSettings?.Enabled != true)
                {
                    throw new ArgumentException("CacheSettings must be enabled to use IpRateLimit");
                }

                services.Configure<IpRateLimitOptions>(Api.ConfigurationRoot.GetSection("IpRateLimiting"));
                services.Configure<IpRateLimitPolicies>(Api.ConfigurationRoot.GetSection("IpRateLimitPolicies"));
                

                if (cacheSettings.UseRedis)
                {
                    services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
                    services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
                }
                else
                {
                    services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                    services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                }

                services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            }
        }
    }
}
