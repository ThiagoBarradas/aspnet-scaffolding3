using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding3.Extensions.Cache
{
    public static class CacheService
    {
        public static void SetupCache(
            this IServiceCollection services,
            CacheSettings cacheSettings)
        {
            if (cacheSettings?.Enabled == true)
            {
                if (cacheSettings.IsDistributed)
                {
                    services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = cacheSettings.RedisConnectionString;
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
