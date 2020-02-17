using AspNetScaffolding;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetScaffolding3.Extensions.GracefullShutdown
{
    public class GracefullShutdownMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly ShutdownSettings ShutdownSettings;

        private readonly GracefullShutdownState State;

        private DateTime ShutdownStarted;

        public GracefullShutdownMiddleware(
            RequestDelegate next,
            IApplicationLifetime applicationLifetime,
            GracefullShutdownState state
        )
        {
            if (applicationLifetime == null)
            {
                throw new ArgumentNullException(nameof(applicationLifetime));
            }

            this.Next = next ?? throw new ArgumentNullException(nameof(next));
            this.ShutdownSettings = Api.ShutdownSettings;
            this.State = state ?? throw new ArgumentNullException(nameof(state));

            applicationLifetime.ApplicationStopping.Register(OnApplicationStopping);
            applicationLifetime.ApplicationStopped.Register(OnApplicationStopped);
        }

        public async Task Invoke(HttpContext context)
        {
            var ignoredRequest = this.State.StopRequested;

            if (!ignoredRequest)
            {
                this.State.NotifyRequestStarted();
            }
                
            try
            {
                await Next.Invoke(context);
            }
            finally
            {
                if (!ignoredRequest)
                {
                    this.State.NotifyRequestFinished();
                }
            }
        }

        private void OnApplicationStopping()
        {
            this.ShutdownStarted = DateTime.UtcNow;
            this.State.NotifyStopRequested();
        }

        private void OnApplicationStopped()
        {
            var shutdownLimit = ShutdownStarted.Add(this.ShutdownSettings.ShutdownTimeoutTimeSpan);

            while (this.State.RequestsInProgress > 0 && DateTime.UtcNow < shutdownLimit)
            {
                this.LogInfo("Application stopping, requests in progress: {RequestsInProgress}", this.State.RequestsInProgress);
                Thread.Sleep(1000);
            }

            if (this.State.RequestsInProgress > 0)
            {
                this.LogError("Application stopped, requests in progress: {RequestsInProgress}", this.State.RequestsInProgress);
            }
            else
            {
                this.LogInfo("Application stopped, requests in progress: {RequestsInProgress}", this.State.RequestsInProgress);
            }

            Log.CloseAndFlush();
        }

        public void LogInfo(string message, long requestsInProgress)
        {
            LogContext.PushProperty("RequestsInProgress", requestsInProgress);
            LogContext.PushProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            Log.Logger.Information(Api.LogSettings.TitlePrefix + " " + message);
        }

        public void LogError(string message, long requestsInProgress)
        {
            LogContext.PushProperty("RequestsInProgress", requestsInProgress);
            LogContext.PushProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            Log.Logger.Error(Api.LogSettings.TitlePrefix + " " + message);
        }
    }
}