using AspNetScaffolding.Extensions.JsonSerializer;
using System;
using TimeZoneConverter;

namespace AspNetScaffolding.Models
{
    public class ApiSettings
    {
        public ApiSettings()
        {
            Domain = "DefaultDomain";
            Application = "DefaultApp";
            SupportedCultures = new string[] { "pt-BR", "en-US" };
            TimezoneDefault = "UTC";
            TimezoneHeader = "Timezone";
            TimeElapsedProperty = "X-Internal-Time";
            RequestKeyProperty = "RequestKey";
        }

        public string AppUrl { get; set; }

        public string PathPrefix { get; set; }

        public string GetPathPrefixConsideringVersion()
        {
            string version = this.Version ?? null;

            return this.PathPrefix.Replace("{version}", version, StringComparison.OrdinalIgnoreCase);
        }

        public string Domain { get; set; }

        public string Application { get; set; }

        public string Version { get; set; }

        public string BuildVersion { get; set; }

        public JsonSerializerEnum JsonSerializer { get; set; }

        public string[] SupportedCultures { get; set; }

        public string RequestKeyProperty { get; set; } 

        public string AccountIdProperty { get; set; }

        public string TimezoneHeader { get; set; }

        public string TimezoneDefault { get; set; }

        public TimeZoneInfo TimezoneDefaultInfo => TZConvert.GetTimeZoneInfo(this.TimezoneDefault);

        public string TimeElapsedProperty { get; set; }
    }
}
