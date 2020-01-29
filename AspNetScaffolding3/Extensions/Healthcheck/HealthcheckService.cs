using AspNetScaffolding.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetScaffolding.Extensions.Healthcheck
{
    public static class HealthcheckServiceExtension
    {
        public static HealthcheckSettings HealthcheckSettings { get; set; }

        public static ApiSettings ApiSettings { get; set; }

        public static void SetupHealthcheck(
            this IServiceCollection services,
            ApiSettings apiSettings,
            HealthcheckSettings healthcheckSettings,
            Action<IHealthChecksBuilder, IServiceProvider> builderFunction)
        {
            HealthcheckSettings = healthcheckSettings;
            ApiSettings = apiSettings;

            if (healthcheckSettings?.Enabled == true)
            {
                var builder = services.AddHealthChecks();
                builderFunction?.Invoke(builder, services.BuildServiceProvider());
            }
        }
    }
}