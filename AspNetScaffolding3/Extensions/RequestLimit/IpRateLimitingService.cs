using AspNetCoreRateLimit;
using AspNetScaffolding.Extensions.Cache;
using AspNetScaffolding3.Extensions.RequestLimit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetScaffolding.Extensions.RequestLimit
{
    public static class IpRateLimitingService
    {
        public static void SetupIpRateLimiting(
            this IServiceCollection services,
            RateLimitingAdditional rateLimitingAdditional,
            CacheSettings cacheSettings)
        {
            if (rateLimitingAdditional?.Enabled == true)
            {
                if (cacheSettings?.Enabled != true)
                {
                    throw new ArgumentException("CacheSettings must be enabled to use IpRateLimit");
                }

                if (rateLimitingAdditional.ByUrlResource)
                {
                    services.Configure<IpRateLimitOptions>(Api.ConfigurationRoot.GetSection("RateLimiting"));
                    services.Configure<IpRateLimitPolicies>(Api.ConfigurationRoot.GetSection("RateLimitPolicies"));
                    services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();
                    UrlResourceRateLimitContributor.UrlResource = rateLimitingAdditional.UrlResource;
                }
                else if (rateLimitingAdditional.ByClientIdHeader)
                {
                    services.Configure<ClientRateLimitOptions>(Api.ConfigurationRoot.GetSection("RateLimiting"));
                    services.Configure<ClientRateLimitPolicies>(Api.ConfigurationRoot.GetSection("ClientRateLimitPolicies"));
                    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                }
                else
                {
                    services.Configure<IpRateLimitOptions>(Api.ConfigurationRoot.GetSection("IpRateLimiting"));
                    services.Configure<IpRateLimitPolicies>(Api.ConfigurationRoot.GetSection("IpRateLimitPolicies"));
                    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                }

                if (cacheSettings.UseRedis)
                {
                    if (rateLimitingAdditional.ByClientIdHeader)
                    {
                        services.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
                    }
                    else
                    {
                        services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
                    }

                    services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
                }
                else
                {
                    if (rateLimitingAdditional.ByClientIdHeader)
                    {
                        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
                    }
                    else
                    {
                        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                    }
                    
                    services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                }
            }
        }
    }
}
