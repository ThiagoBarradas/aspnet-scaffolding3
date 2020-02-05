using AspNetScaffolding.Extensions.AccountId;
using AspNetScaffolding.Extensions.Cors;
using AspNetScaffolding.Extensions.CultureInfo;
using AspNetScaffolding.Extensions.Docs;
using AspNetScaffolding.Extensions.ExceptionHandler;
using AspNetScaffolding.Extensions.Healthcheck;
using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Extensions.Logger;
using AspNetScaffolding.Extensions.Mapper;
using AspNetScaffolding.Extensions.QueryFormatter;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Extensions.RoutePrefix;
using AspNetScaffolding.Extensions.TimeElapsed;
using AspNetScaffolding.Utilities;
using AspNetSerilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp.Serilog.Auto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AspNetScaffolding
{
    public class Startup
    {
        public Startup()
        {
            var envName = EnvironmentUtility.GetCurrentEnvironment();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(Api.ApiBasicConfiguration?.EnvironmentVariablesPrefix);

            Api.ConfigurationRoot = builder.Build();

            Api.ConfigurationRoot.GetSection("ApiSettings").Bind(Api.ApiSettings);
            Api.ConfigurationRoot.GetSection("HealthcheckSettings").Bind(Api.HealthcheckSettings);
            Api.ConfigurationRoot.GetSection("LogSettings").Bind(Api.LogSettings);
            Api.ConfigurationRoot.GetSection("DatabaseSettings").Bind(Api.DatabaseSettings);
            Api.ConfigurationRoot.GetSection("DocsSettings").Bind(Api.DocsSettings);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Api.ConfigurationRoot);
            services.AddHttpContextAccessor();
            
            services.SetupSwaggerDocs(Api.DocsSettings, Api.ApiSettings);
            
            var mvc = services.AddMvc(options => options.EnableEndpointRouting = false);
            mvc.RegisterAssembliesForControllers(Api.ApiBasicConfiguration?.AutoRegisterAssemblies);
            mvc.RegisterAssembliesForFluentValidation(Api.ApiBasicConfiguration?.AutoRegisterAssemblies);
            mvc.ConfigureJsonSettings(services,
                Api.ApiSettings.JsonSerializer,
                Api.ApiSettings?.TimezoneHeader,
                Api.ApiSettings?.TimezoneDefaultInfo);
            
            mvc.AddMvcOptions(options =>
            {
                options.UseCentralRoutePrefix(Api.ApiSettings.GetPathPrefixConsideringVersion());
                options.AddQueryFormatter(Api.ApiSettings.JsonSerializer);
                options.AddPathFormatter(Api.ApiSettings.JsonSerializer);
            });
            
            services.AddScoped<IRestClientFactory, RestClientFactory>();
            
            services.SetupAllowCors();
            services.SetupRequestKey(Api.ApiSettings?.RequestKeyProperty);
            services.SetupAccountId(Api.ApiSettings?.AccountIdProperty);
            services.SetupTimeElapsed(Api.ApiSettings?.TimeElapsedProperty);
            
            
            List<string> ignoredRoutes = Api.DocsSettings.GetDocsFinalRoutes().ToList();
            
            if (Api.HealthcheckSettings.LogEnabled == false)
            {
                ignoredRoutes.Add(HealthcheckkMiddlewareExtension.GetFullPath(Api.ApiSettings, Api.HealthcheckSettings));
            }
            
            services.SetupSerilog(Api.ApiSettings?.Domain,
                Api.ApiSettings?.Application,
                Api.LogSettings, 
                ignoredRoutes);
            
            services.AddControllers();

            Api.ApiBasicConfiguration.ConfigureServices?.Invoke(services);

            services.SetupAutoMapper();

            services.SetupHealthcheck(Api.ApiSettings,
                Api.HealthcheckSettings,
                Api.ApiBasicConfiguration.ConfigureHealthcheck);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAspNetSerilog();
            app.UseAccountId();
            app.UseRequestKey();
            app.UseTimeElapsed();
            app.UseScaffoldingSwagger();
            app.UseScaffoldingRequestLocalization(Api.ApiSettings?.SupportedCultures);
            app.UseScaffoldingExceptionHandler();
            app.UseHealthcheck();
            app.UseMvc();
            app.AllowCors();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Api.ApiBasicConfiguration.Configure?.Invoke(app);
        }

        public static void ConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            builder.AddUrlGroup(new Uri("https://www.google.com"), "google");
        }
    }
}