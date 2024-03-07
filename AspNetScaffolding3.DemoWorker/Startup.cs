using System;
using System.Diagnostics.CodeAnalysis;
using AspNetScaffolding.Extensions.Queue;
using AspNetScaffolding3.DemoWorker.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding3.DemoWorker
{
    [ExcludeFromCodeCoverage]
    public static class Startup
    {
        public static void ConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            QueueHealthcheck.AddRabbitMqAutomatic(builder, provider);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .SetupServicesConfiguration()
                .SetupQueueConfiguration();
        }

        public static void Configure(IApplicationBuilder app)
        {
        }

        private static IServiceCollection SetupServicesConfiguration(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection SetupQueueConfiguration(this IServiceCollection services) 
        {
            #region -- Workers --

            services.SetupWorker<WorkerRunner>();

            #endregion

            return services;
        }
    }
}