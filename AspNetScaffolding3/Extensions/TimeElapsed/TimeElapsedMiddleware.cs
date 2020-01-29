using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.TimeElapsed
{
    public class TimeElapsedMiddleware
    {
        private readonly RequestDelegate Next;

        public TimeElapsedMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            string timeIsMs = "-1";

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[TimeElapsedServiceExtension.TimeElapsedHeaderName] = timeIsMs;
                return Task.CompletedTask;
            });

            stopwatch.Start();

            await this.Next(context);

            stopwatch.Stop();
            timeIsMs = stopwatch.ElapsedMilliseconds.ToString();
            context.Items[TimeElapsedServiceExtension.TimeElapsedHeaderName] = timeIsMs;
        }
    }

    public static class TimeElapsedMiddlewareExtension
    {
        public static void UseTimeElapsed(this IApplicationBuilder app)
        {
            app.UseMiddleware<TimeElapsedMiddleware>();
        }
    }
}
