using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.TimeElapsed
{
    public static class TimeElapsedServiceExtension
    {
        public static string TimeElapsedHeaderName = "X-Internal-Time";

        public static void SetupTimeElapsed(this IServiceCollection services, string headerName = null)
        {
            if (string.IsNullOrWhiteSpace(headerName) == false)
            {
                TimeElapsedHeaderName = headerName;
            }
        }
    }
}
