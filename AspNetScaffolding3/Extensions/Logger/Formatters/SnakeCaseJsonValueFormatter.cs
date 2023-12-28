using Newtonsoft.Json.Serialization;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.IO;

namespace AspNetScaffolding3.Extensions.Logger.Formatters
{
    internal class SnakeCaseJsonValueFormatter : JsonValueFormatter
    {
        private readonly string _typeTagName;
        private readonly SnakeCaseNamingStrategy _strategy;

        public SnakeCaseJsonValueFormatter(SnakeCaseNamingStrategy snakeCaseStrategy, string typeTagName = "_typeTag") : base(typeTagName)
        {
            _typeTagName = typeTagName;
            _strategy = snakeCaseStrategy;
        }

        protected override bool VisitStructureValue(TextWriter state, StructureValue structure)
        {
            state.Write('{');
            string value = "";
            for (int i = 0; i < structure.Properties.Count; i++)
            {
                state.Write(value);
                value = ",";
                LogEventProperty logEventProperty = structure.Properties[i];
                WriteQuotedJsonString(_strategy.GetPropertyName(logEventProperty.Name, false), state);
                state.Write(':');
                Visit(state, logEventProperty.Value);
            }

            if (_typeTagName != null && structure.TypeTag != null)
            {
                state.Write(value);
                WriteQuotedJsonString(_typeTagName, state);
                state.Write(':');
                WriteQuotedJsonString(structure.TypeTag, state);
            }

            state.Write('}');
            return false;
        }
    }
}
