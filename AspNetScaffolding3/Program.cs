using AspNetScaffolding.Models;
using System.Net;

namespace AspNetScaffolding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var config = new ApiBasicConfiguration
            {
                ApiName = "My AspNet Scaffolding",
                ApiPort = 8700,
                EnvironmentVariablesPrefix = "Prefix_",
                ConfigureHealthcheck = Startup.ConfigureHealthcheck
            };

            Api.Run(config);
        }
    }
}
