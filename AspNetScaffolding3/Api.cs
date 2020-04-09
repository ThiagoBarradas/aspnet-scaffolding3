using AspNetScaffolding.Extensions.Docs;
using AspNetScaffolding.Extensions.GracefullShutdown;
using AspNetScaffolding.Extensions.Healthcheck;
using AspNetScaffolding.Extensions.Logger;
using AspNetScaffolding.Models;
using AspNetScaffolding.Extensions.Cache;
using AspNetScaffolding.Extensions.RequestLimit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AspNetScaffolding
{
    public static class Api
    {
        public static ApiBasicConfiguration ApiBasicConfiguration { get; set; } = new ApiBasicConfiguration();

        public static IConfigurationRoot ConfigurationRoot { get; set; }

        public static ApiSettings ApiSettings { get; set; } = new ApiSettings();

        public static HealthcheckSettings HealthcheckSettings { get; set; } = new HealthcheckSettings();

        public static LoggerSettings LogSettings { get; set; } = new LoggerSettings();

        public static DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();

        public static DocsSettings DocsSettings { get; set; } = new DocsSettings();

        public static ShutdownSettings ShutdownSettings { get; set; } = new ShutdownSettings();

        public static IpRateLimitingAdditional IpRateLimitingAdditional { get; set; } = new IpRateLimitingAdditional();

        public static CacheSettings CacheSettings { get; set; } = new CacheSettings();

        public static void Run(ApiBasicConfiguration apiBasicConfiguration)
        {
            ApiBasicConfiguration = apiBasicConfiguration;

            Console.WriteLine("{0} is running...", ApiBasicConfiguration.ApiName);

            var host = new WebHostBuilder()
                .UseKestrel(options => options.AllowSynchronousIO = true)
                .UseUrls("http://*:" + ApiBasicConfiguration.ApiPort.ToString())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();

            host.Build().Run();
        }
    }
}
