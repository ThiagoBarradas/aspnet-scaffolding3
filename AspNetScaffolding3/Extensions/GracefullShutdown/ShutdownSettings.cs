using System;

namespace AspNetScaffolding.Extensions.GracefullShutdown
{
    public class ShutdownSettings
    {
        /// <summary>
        /// enables gracefull shutdown
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// false - incoming requests will get 500 code after shutdown initiation
        /// true  - incoming requests will be redirected with 308 code, and same url
        /// </summary>
        public bool Redirect { get; set; }

        /// <summary>
        /// forces shutdown after X seconds
        /// </summary>
        public int ShutdownTimeoutInSeconds { get; set; } = 60;

        /// <summary>
        /// forces shutdown after X seconds - timespan format
        /// </summary>
        public TimeSpan ShutdownTimeoutTimeSpan => TimeSpan.FromSeconds(ShutdownTimeoutInSeconds);

        /// <summary>
        /// State for gracefull shutdown
        /// </summary>
        public GracefullShutdownState GracefullShutdownState { get; set; } = new GracefullShutdownState();
    }
}
