using AspNetScaffolding.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.Cache
{
    public static class CacheService
    {
        public static void SetupCache(
            this IServiceCollection services,
            CacheSettings cacheSettings,
            ApiSettings apiSettings)
        {
            if (cacheSettings?.Enabled == true)
            {
                if (cacheSettings.UseRedis)
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = cacheSettings.GetCacheConnectionString();
                        options.InstanceName = cacheSettings.CachePrefix;
                    });
                }
                else
                {
                    services.AddMemoryCache();
                }
            }
        }
    }
}
