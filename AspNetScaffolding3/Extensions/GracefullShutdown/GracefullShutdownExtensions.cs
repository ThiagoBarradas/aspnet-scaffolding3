using AspNetScaffolding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Models.Exceptions;

namespace AspNetScaffolding3.Extensions.GracefullShutdown
{
    public static class GracefullShutdownExtensions
    {
        public static void ValidationShutdown(this ControllerBase controller)
        {
            if (Api.ShutdownSettings?.Enabled == false)
            {
                if (Api.ShutdownSettings.GracefullShutdownState.StopRequested && Api.ShutdownSettings.Redirect)
                {
                    throw new PermanentRedirectException(null);
                }
                else if (Api.ShutdownSettings.GracefullShutdownState.StopRequested)
                {
                    throw new ServiceUnavailableException("Service is unavailable for temporary maintenance");
                }
            }
        }

        public static IApplicationBuilder UseGracefullShutdown(this IApplicationBuilder builder)
        {
            if (Api.ShutdownSettings?.Enabled != true)
            {
                return builder;
            }

            return builder.UseMiddleware<GracefullShutdownMiddleware>();
        }

        public static IServiceCollection AddGracefullShutdown(this IServiceCollection services)
        {
            services.AddSingleton(Api.ShutdownSettings.GracefullShutdownState);
            services.AddSingleton<IRequestsCountProvider>(Api.ShutdownSettings.GracefullShutdownState);

            return services;
        }
    }
}
