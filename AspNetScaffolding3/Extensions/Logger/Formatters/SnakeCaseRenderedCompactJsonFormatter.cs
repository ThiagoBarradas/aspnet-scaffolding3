using Newtonsoft.Json.Serialization;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Formatting;
using System.Collections.Generic;
using System.IO;
using System;

namespace AspNetScaffolding3.Extensions.Logger.Formatters
{
    internal class SnakeCaseRenderedCompactJsonFormatter : ITextFormatter
    {
        private readonly SnakeCaseJsonValueFormatter _valueFormatter;
        private static readonly SnakeCaseNamingStrategy _strategy = new SnakeCaseNamingStrategy();

        //
        // Summary:
        //     Construct a Serilog.Formatting.Compact.CompactJsonFormatter, optionally supplying
        //     a formatter for Serilog.Events.LogEventPropertyValues on the event.
        //
        // Parameters:
        //   valueFormatter:
        //     A value formatter, or null.
        public SnakeCaseRenderedCompactJsonFormatter(SnakeCaseJsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new SnakeCaseJsonValueFormatter(_strategy, "$type");
        }

        //
        // Summary:
        //     Format the log event into the output. Subsequent events will be newline-delimited.
        //
        //
        // Parameters:
        //   logEvent:
        //     The event to format.
        //
        //   output:
        //     The output.
        public void Format(LogEvent logEvent, TextWriter output)
        {
            FormatEvent(logEvent, output, _valueFormatter);
            output.WriteLine();
        }

        //
        // Summary:
        //     Format the log event into the output.
        //
        // Parameters:
        //   logEvent:
        //     The event to format.
        //
        //   output:
        //     The output.
        //
        //   valueFormatter:
        //     A value formatter for Serilog.Events.LogEventPropertyValues on the event.
        public static void FormatEvent(LogEvent logEvent, TextWriter output, SnakeCaseJsonValueFormatter valueFormatter)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (valueFormatter == null)
            {
                throw new ArgumentNullException("valueFormatter");
            }

            output.Write("{\"@t\":\"");
            output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));
            output.Write("\",\"@m\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);
            output.Write(",\"@i\":\"");
            output.Write(EventIdHash.Compute(logEvent.MessageTemplate.Text).ToString("x8"));
            output.Write('"');
            if (logEvent.Level != LogEventLevel.Information)
            {
                output.Write(",\"@l\":\"");
                output.Write(logEvent.Level);
                output.Write('"');
            }

            if (logEvent.Exception != null)
            {
                output.Write(",\"@x\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
            {
                string text = _strategy.GetPropertyName(property.Key, false);
                if (text.Length > 0 && text[0] == '@')
                {
                    text = "@" + text;
                }

                output.Write(',');
                JsonValueFormatter.WriteQuotedJsonString(text, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output);
            }

            output.Write('}');
        }
    }
}
