using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.Cors
{
    public static class CorsServiceExtension
    {
        public const string CorsName = "EnableAll";

        public static void SetupAllowCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsName, builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
        }
    }
}
