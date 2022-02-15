using AspNetScaffolding.Extensions.Cache;
using AspNetScaffolding.Extensions.Docs;
using AspNetScaffolding.Extensions.GracefullShutdown;
using AspNetScaffolding.Extensions.Healthcheck;
using AspNetScaffolding.Extensions.Logger;
using AspNetScaffolding.Extensions.Queue;
using AspNetScaffolding.Extensions.RequestLimit;
using AspNetScaffolding.Extensions.Worker;
using AspNetScaffolding.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using AspNetScaffolding3.Extensions.RequestLimit;

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

        public static QueueSettings QueueSettings { get; set; } = new QueueSettings();

        public static RateLimitingAdditional RateLimitingAdditional { get; set; } = new RateLimitingAdditional();

        public static CacheSettings CacheSettings { get; set; } = new CacheSettings();

        public static WorkerSettings WorkerSettings { get; set; } = new WorkerSettings();

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
