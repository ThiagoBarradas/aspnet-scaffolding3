using AspNetScaffolding.Models;
using System.Reflection;

namespace $ext_safeprojectname$
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ApiBasicConfiguration
            {
                ApiName = "My AspNet Scaffolding",
                ApiPort = 80,
                EnvironmentVariablesPrefix = "Prefix_",
                ConfigureHealthcheck = Startup.ConfigureHealthcheck,
                ConfigureServices = Startup.ConfigureServices,
                Configure = Startup.Configure,
                AutoRegisterAssemblies = new Assembly[]
                   { Assembly.GetExecutingAssembly() }
            };

            AspNetScaffolding.Api.Run(config);
        }
    }
}
