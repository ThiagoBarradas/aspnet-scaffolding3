using Serilog.Builder.Models;
using System;
using System.Linq;
using AspNetSerilog;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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

        public Dictionary<string, Func<string, string>> JsonBlacklistRequestPartial { get; set; }

        public string[] JsonBlacklistResponse { get; set; }

        public Dictionary<string, Func<string, string>> JsonBlacklistResponsePartial { get; set; }

        public string[] HeaderBlacklist { get; set; }
        
        public string[] HttpContextBlacklist { get; set; }

        public string InformationTitle { get; set; }

        public string ErrorTitle { get; set; }
        
        public string[] QueryStringBlacklist { get; set; }

        public bool DebugEnabled { get; set; }

        public SeqOptions SeqOptions { get; set; } = new SeqOptions();

        public SplunkOptions SplunkOptions { get; set; } = new SplunkOptions();

        public NewRelicOptions NewRelicOptions { get; set; } = new NewRelicOptions();

        public DataDogOptions DataDogOptions { get; set; } = new DataDogOptions();

        public LapiOptions LapiOptions { get; set; } = new LapiOptions();
        
        public AspNetScaffolding.Extensions.Logger.ScaffoldingConsoleOptions ConsoleOptions { get; set; } = new AspNetScaffolding.Extensions.Logger.ScaffoldingConsoleOptions();

        public Action<IServiceCollection, SerilogConfiguration> SetupSerilog { get; set; }

        public string GetInformationTitle()
        {
            if (string.IsNullOrWhiteSpace(InformationTitle))
                return CommunicationLogger.DefaultInformationTitle;

            return InformationTitle;
        }
        
        public string GetErrorTitle()
        {
            if (string.IsNullOrWhiteSpace(ErrorTitle))
                return CommunicationLogger.DefaultErrorTitle;

            return ErrorTitle;
        }
    }

    public class ScaffoldingConsoleOptions : Serilog.Builder.Models.ConsoleOptions
    {
        public EConsoleEnricherFormatter FormatterType { get; set; } = EConsoleEnricherFormatter.Default;
    }

    public enum EConsoleEnricherFormatter
    {
        Default,
        SnakeCase
    }
}
