using Serilog.Builder.Models;
using System;
using System.Linq;

namespace AspNetScaffolding.Extensions.Logger
{
    public class LoggerSettings
    {
        public string TitlePrefix { get; set; }

        /// <summary>
        /// Previously JsonBlacklistRequest
        /// </summary>
        [Obsolete]
        public string[] JsonBlacklist { get; set; }

        public string[] GetJsonBlacklistRequest()
        {
            if (this.JsonBlacklistRequest?.Any() == true)
            {
                return this.JsonBlacklistRequest;
            }

            return this.JsonBlacklist;
        }

        public string[] JsonBlacklistRequest { get; set; }

        public string[] JsonBlacklistResponse { get; set; }

        public bool DebugEnabled { get; set; }

        public SeqOptions SeqOptions { get; set; } = new SeqOptions();

        public SplunkOptions SplunkOptions { get; set; } = new SplunkOptions();

        public NewRelicOptions NewRelicOptions { get; set; } = new NewRelicOptions();

        public DataDogOptions DataDogOptions { get; set; } = new DataDogOptions();
    }
}
