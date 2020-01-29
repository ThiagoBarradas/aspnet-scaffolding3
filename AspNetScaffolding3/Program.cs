using AspNetScaffolding.Models;

namespace AspNetScaffolding
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
                ConfigureHealthcheck = Startup.ConfigureHealthcheck
            };

            Api.Run(config);
        }
    }
}
