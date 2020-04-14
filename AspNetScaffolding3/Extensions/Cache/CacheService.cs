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
                if (cacheSettings.IsDistributed)
                {
                    services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = cacheSettings.RedisConnectionString;
                        options.InstanceName = apiSettings.Application;
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
