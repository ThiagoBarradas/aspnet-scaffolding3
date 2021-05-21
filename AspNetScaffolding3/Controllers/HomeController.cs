using AspNetScaffolding.Extensions.GracefullShutdown;
using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Utilities;
using AspNetSerilog.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackUtils.Converters;
using System;

namespace AspNetScaffolding.Controllers
{
    public class HomeController : BaseController
    {
        protected readonly RequestKey RequestKey;

        protected readonly IHttpContextAccessor HttpContextAccessor;

        protected readonly GracefullShutdownState GracefullShutdownState;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            GracefullShutdownState gracefullShutdownState,
            RequestKey requestKey)
        {
            HttpContextAccessor = httpContextAccessor;
            GracefullShutdownState = gracefullShutdownState;
            RequestKey = requestKey;
        }

        [HttpGet]
        public IActionResult GetAppInfo()
        {
            this.DisableLogging();

            return Ok(new HomeDetails
            {
                RIP = GracefullShutdownState.RequestsInProgress,
                Service = Api.ApiBasicConfiguration?.ApiName,
                BuildVersion = Api.ApiSettings?.BuildVersion,
                Environment = EnvironmentUtility.GetCurrentEnvironment(),
                RequestKey = RequestKey.Value,
                Application = Api.ApiSettings.Application,
                Domain = Api.ApiSettings.Domain,
                JsonSerializer = Api.ApiSettings.JsonSerializer,
                EnvironmentPrefix = Api.ApiBasicConfiguration.EnvironmentVariablesPrefix,
                TimezoneInfo = new TimezoneInfo(HttpContextAccessor)
            });
        }

        public class HomeDetails
        {
            public string Service { get; set; }

            public string BuildVersion { get; set; }

            public string Environment { get; set; }

            public string Application { get; set; }

            public string Domain { get; set; }

            public string EnvironmentPrefix { get; set; }

            public long RIP { get; set; }

            public JsonSerializerEnum JsonSerializer { get; set; }

            public string RequestKey { get; set; }

            public TimezoneInfo TimezoneInfo { get; set; }
        }

        public class TimezoneInfo
        {
            public TimezoneInfo(IHttpContextAccessor httpContextAccessor)
            {
                CurrentTimezone = DateTimeConverter.GetTimeZoneByAspNetHeader(
                    httpContextAccessor,
                    Api.ApiSettings.TimezoneHeader).Id;
            }

            public string UtcNow => DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

            public DateTime CurrentNow => DateTime.UtcNow;

            public string DefaultTimezone => Api.ApiSettings.TimezoneDefaultInfo.Id;

            public string CurrentTimezone { get; set; }

            public string TimezoneHeader => Api.ApiSettings.TimezoneHeader;
        }
    }
}
