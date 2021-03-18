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
            TimezoneDefault = "UTC";
            TimezoneHeader = "Timezone";
            TimeElapsedProperty = "X-Internal-Time";
            RequestKeyProperty = "RequestKey";
        }

        public string AppUrl { get; set; }

        public string PathPrefix { get; set; }

        public string GetPathPrefixConsideringVersion()
        {
            string version = Version ?? null;

            return PathPrefix.Replace("{version}", version, StringComparison.OrdinalIgnoreCase);
        }

        public string Domain { get; set; }

        public string Application { get; set; }

        public string Version { get; set; }

        public string BuildVersion { get; set; }

        public JsonSerializerEnum JsonSerializer { get; set; }

        public string JsonSerializerString => Api.ApiSettings.JsonSerializer.ToString().ToLower();

        public string[] SupportedCultures { get; set; }

        public string RequestKeyProperty { get; set; }

        public string AccountIdProperty { get; set; }

        public bool UseStaticFiles { get; set; }
        
        public string StaticFilesPath { get; set; }

        public string GetStaticFilesPath()
        {
            return $"/{this.GetPathPrefixConsideringVersion().TrimEnd('/')}/{this.StaticFilesPath}".Replace("//","/").TrimEnd('/');
        }

        public string TimezoneHeader { get; set; }

        public string TimezoneDefault { get; set; }

        public TimeZoneInfo TimezoneDefaultInfo => TZConvert.GetTimeZoneInfo(TimezoneDefault);

        public string TimeElapsedProperty { get; set; }

        public string JsonFieldSelectorProperty { get; set; }

        public bool IsJsonFieldSelectorEnabled => !string.IsNullOrWhiteSpace(this.JsonFieldSelectorProperty);
    }
}
