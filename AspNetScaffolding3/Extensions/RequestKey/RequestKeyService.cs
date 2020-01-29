using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.RequestKey
{
    public static class RequestKeyServiceExtension
    {
        public static string RequestKeyHeaderName = "RequestKey";

        public static void SetupRequestKey(this IServiceCollection services, string headerName = null)
        {
            if (string.IsNullOrWhiteSpace(headerName) == false)
            {
                RequestKeyHeaderName = headerName;
            }

            services.AddScoped<RequestKeyMiddleware>();
            services.AddScoped(obj => new RequestKey());
        }
    }
}
