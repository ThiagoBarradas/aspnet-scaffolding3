using System;

namespace AspNetScaffolding.Utilities
{
    public static class EnvironmentUtility
    {
        public static string GetCurrentEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }

        public static bool IsDevelopment()
        {
            var env = GetCurrentEnvironment();

            return env.Contains("develop");
        }
    }
}
