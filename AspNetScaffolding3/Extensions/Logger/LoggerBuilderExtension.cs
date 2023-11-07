using Serilog;
using Serilog.Builder.Models;
using Serilog.Formatting.Json;

namespace AspNetScaffolding3.Extensions.Logger
{
    public static class LoggerBuilderExtension
    {

        public static LoggerConfiguration EnableStdOutput(this LoggerConfiguration loggerConfiguration, ConsoleOptions consoleOptions = null)
        {
            if (consoleOptions?.Enabled ?? false)
            {
                loggerConfiguration.WriteTo.Console(formatter: new JsonFormatter())
                    .Enrich.FromLogContext();
            }

            return loggerConfiguration;
        }
    }
}