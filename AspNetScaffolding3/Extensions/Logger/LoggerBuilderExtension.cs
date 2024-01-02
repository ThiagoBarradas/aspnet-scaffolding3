using AspNetScaffolding.Extensions.Logger;

using AspNetScaffolding3.Extensions.Logger.Formatters;

using Serilog;
using Serilog.Formatting.Json;

namespace AspNetScaffolding3.Extensions.Logger
{
    public static class LoggerBuilderExtension
    {
        public static LoggerConfiguration EnableStdOutput(this LoggerConfiguration loggerConfiguration, ScaffoldingConsoleOptions consoleOptions = null)
        {
            if (consoleOptions?.Enabled ?? false)
            {
                return consoleOptions.FormatterType switch
                {
                    EConsoleEnricherFormatter.SnakeCase => loggerConfiguration.WriteTo.Console(new SnakeCaseRenderedCompactJsonFormatter()).Enrich.FromLogContext(),
                    _ => loggerConfiguration.WriteTo.Console(formatter: new JsonFormatter()).Enrich.FromLogContext(),
                };
            }

            return loggerConfiguration;
        }
    }
}