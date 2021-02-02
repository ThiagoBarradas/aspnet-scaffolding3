using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Models;
using AspNetScaffolding3.DemoApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing.Constraints;
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
                ConfigureHealthcheck = ConfigureHealthcheck,
                ConfigureServices = ConfigureServices,
                Configure = Configure,
                ConfigureAfter = ConfigureAfter,
                AutoRegisterAssemblies = new Assembly[]
                    { Assembly.GetExecutingAssembly() }
            };

            Api.Run(config);
        }

        public static void ConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            // add health check configuration
            builder.AddUrlGroup(new Uri("https://www.google.com"), "google");
            //builder.AddMongoDb("mongodb://localhost:27017");
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var apiUrl = "https://api.mundipagg.com/core/v1";

            services.AddScoped<IMundipaggApiClient>(provider =>
            {
                return new MundipaggApiClient(null, provider.GetService<RequestKey>().Value, apiUrl, 120000);
            });

            // add services
            //services.AddSingleton<ISomething, Something>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            // customize your app
            //app.UseAuthentication();
        }

        public static void ConfigureAfter(IApplicationBuilder app)
        {
            var prefix = Api.ApiSettings.GetPathPrefixConsideringVersion();

            var controllerName = nameof(ProxyController).Replace("Controller", "");
            var actionName = nameof(ProxyController.HandleAllRequests);

            var method = "POST";
            var path = prefix + "/transactions/{id}";
            var resourceName = "transactions.create";

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(resourceName, path,
                    defaults: new
                    {
                        controller = controllerName,
                        action = actionName,
                        resource = resourceName
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodRouteConstraint(method)
                    });
                endpoints.MapControllerRoute("xpto", prefix + "/xpto",
                    defaults: new
                    {
                        controller = controllerName,
                        action = actionName,
                        resource = "xpto"
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodRouteConstraint("GET")
                    });
            });
        }
    }
}
