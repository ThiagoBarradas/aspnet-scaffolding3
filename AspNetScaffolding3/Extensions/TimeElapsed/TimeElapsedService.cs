using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading.Tasks;

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
