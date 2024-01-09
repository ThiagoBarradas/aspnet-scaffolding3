using AspNetScaffolding.Extensions.Logger;

using AspNetScaffolding3.Extensions.Logger.Formatters;

using Serilog;
using Serilog.Builder;
using Serilog.Formatting.Json;

namespace AspNetScaffolding3.Extensions.Logger
{
    public static class LoggerBuilderExtension
    {
        public static LoggerBuilder DisableConsoleIfConsoleSinkIsEnabled(this LoggerBuilder loggerBuilder, ScaffoldingConsoleOptions? consoleOptions = null)
        {
            if (consoleOptions?.Enabled ?? false)
            {
                loggerBuilder.DisableConsole();
            }

            return loggerBuilder;
        }

        public static LoggerConfiguration EnableStdOutput(this LoggerConfiguration loggerConfiguration, ScaffoldingConsoleOptions consoleOptions = null)
        {
            if (consoleOptions?.Enabled ?? false)
            {
                var minimmumLevel = consoleOptions.MinimumLevel ?? Serilog.Events.LogEventLevel.Verbose;
                return consoleOptions.FormatterType switch
                {
                    EConsoleEnricherFormatter.SnakeCase => loggerConfiguration.WriteTo.Console(new SnakeCaseRenderedCompactJsonFormatter(), minimmumLevel).Enrich.FromLogContext(),
                    _ => loggerConfiguration.WriteTo.Console(formatter: new JsonFormatter(), minimmumLevel).Enrich.FromLogContext(),
                };
            }

            return loggerConfiguration;
        }
    }
}