using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspNetScaffolding.Models
{
    public class ApiBasicConfiguration
    {
        public string ApiName { get; set; }

        public int ApiPort { get; set; }

        public string EnvironmentVariablesPrefix { get; set; }

        public IEnumerable<Assembly> AutoRegisterAssemblies { get; set; } = new List<Assembly>();

        public Action<IHealthChecksBuilder, IServiceProvider> ConfigureHealthcheck { get; set; }

        public Action<IServiceCollection> ConfigureServices { get; set; }

        public Action<IApplicationBuilder> Configure { get; set; }
    }
}
