using System.Reflection;
using AspNetScaffolding;
using AspNetScaffolding.Models;

namespace AspNetScaffolding3.DemoWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ApiBasicConfiguration
            {
                ApiName = "Demo Worker",
                ApiPort = 8701,
                EnvironmentVariablesPrefix = "APP_",
                ConfigureHealthcheck = Startup.ConfigureHealthcheck,
                ConfigureServices = Startup.ConfigureServices,
                Configure = Startup.Configure,
                AutoRegisterAssemblies = new Assembly[]
                    { Assembly.GetExecutingAssembly() }
            };

            Api.Run(config);
        }
    }
}