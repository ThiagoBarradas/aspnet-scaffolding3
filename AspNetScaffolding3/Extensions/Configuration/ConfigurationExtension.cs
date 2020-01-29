using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.Configuration
{
    public static class ConfigurationExtension
    {
        public static void AddSingletonConfiguration<T>(this IServiceCollection services, string section) where T : class, new()
        {
            T currentObject = new T();
            Api.ConfigurationRoot.GetSection(section).Bind(currentObject);
            services.AddSingleton(obj => currentObject);
        }
    }
}
