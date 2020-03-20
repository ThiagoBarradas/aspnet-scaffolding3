using AspNetScaffolding;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.GracefullShutdown
{
    public class GracefullShutdownMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly ShutdownSettings ShutdownSettings;

        private readonly GracefullShutdownState State;

        private DateTime ShutdownStarted;

        public GracefullShutdownMiddleware(
            RequestDelegate next,
            IHostApplicationLifetime applicationLifetime,
            GracefullShutdownState state
        )
        {
            if (applicationLifetime == null)
            {
                throw new ArgumentNullException(nameof(applicationLifetime));
            }

            Next = next ?? throw new ArgumentNullException(nameof(next));
            ShutdownSettings = Api.ShutdownSettings;
            State = state ?? throw new ArgumentNullException(nameof(state));

            applicationLifetime.ApplicationStopping.Register(OnApplicationStopping);
            applicationLifetime.ApplicationStopped.Register(OnApplicationStopped);
        }

        public async Task Invoke(HttpContext context)
        {
            var ignoredRequest = State.StopRequested;

            if (!ignoredRequest)
            {
                State.NotifyRequestStarted();
            }

            try
            {
                await Next.Invoke(context);
            }
            finally
            {
                if (!ignoredRequest)
                {
                    State.NotifyRequestFinished();
                }
            }
        }

        private void OnApplicationStopping()
        {
            ShutdownStarted = DateTime.UtcNow;
            State.NotifyStopRequested();
        }

        private void OnApplicationStopped()
        {
            var shutdownLimit = ShutdownStarted.Add(ShutdownSettings.ShutdownTimeoutTimeSpan);

            while (State.RequestsInProgress > 0 && DateTime.UtcNow < shutdownLimit)
            {
                LogInfo("Application stopping, requests in progress: {RequestsInProgress}", State.RequestsInProgress);
                Thread.Sleep(1000);
            }

            if (State.RequestsInProgress > 0)
            {
                LogError("Application stopped, requests in progress: {RequestsInProgress}", State.RequestsInProgress);
            }
            else
            {
                LogInfo("Application stopped, requests in progress: {RequestsInProgress}", State.RequestsInProgress);
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