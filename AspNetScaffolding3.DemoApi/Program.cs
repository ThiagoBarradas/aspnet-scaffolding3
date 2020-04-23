using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mundipagg;
using System;
using System.Reflection;

namespace AspNetScaffolding.DemoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ApiBasicConfiguration
            {
                ApiName = "My AspNet Scaffolding",
                ApiPort = 8700,
                EnvironmentVariablesPrefix = "Prefix_",
                ConfigureHealthcheck = AdditionalConfigureHealthcheck,
                ConfigureServices = AdditionalConfigureServices,
                Configure = AdditionalConfigure,
                AutoRegisterAssemblies = new Assembly[]
                    { Assembly.GetExecutingAssembly() }
            };

            Api.Run(config);
        }

        public static void AdditionalConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            // add health check configuration
            builder.AddUrlGroup(new Uri("https://www.google.com"), "google");
            //builder.AddMongoDb("mongodb://localhost:27017");
        }

        public static void AdditionalConfigureServices(IServiceCollection services)
        {
            var apiUrl = "https://api.mundipagg.com/core/v1";

            services.AddScoped<IMundipaggApiClient>(provider =>
            {
                return new MundipaggApiClient(null, provider.GetService<RequestKey>().Value, apiUrl, 120000);
            });

            // add services
            //services.AddSingleton<ISomething, Something>();
        }

        public static void AdditionalConfigure(IApplicationBuilder app)
        {
            // customize your app
            //app.UseAuthentication();
        }

    }
}
