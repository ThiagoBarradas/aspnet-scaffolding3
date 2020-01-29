using Serilog.Builder.Models;

namespace AspNetScaffolding.Extensions.Logger
{
    public class LoggerSettings
    {
        public string TitlePrefix { get; set; }

        public string[] JsonBlacklist { get; set; }

        public bool DebugEnabled { get; set; }

        public SeqOptions SeqOptions { get; set; } = new SeqOptions();

        public SplunkOptions SplunkOptions { get; set; } = new SplunkOptions();
    }
}
