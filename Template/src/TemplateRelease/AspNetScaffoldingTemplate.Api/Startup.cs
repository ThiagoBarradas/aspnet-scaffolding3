using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace $ext_safeprojectname$
{
    public static class Startup
    {
        public static void ConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            // add health check configuration
            builder.AddUrlGroup(new Uri("https://www.google.com"), "google");    
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // add services
            //services.AddSingleton<ISomething, Something>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            // customize your app
        }
    }
}
