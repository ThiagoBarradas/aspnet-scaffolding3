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
    [ApiController]
    public class HomeController : BaseController
    {
        protected readonly RequestKey RequestKey;

        protected readonly IHttpContextAccessor HttpContextAccessor;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            RequestKey requestKey)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.RequestKey = requestKey;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(HomeDetails), 200)]
        public IActionResult Home()
        {
            this.DisableLogging();

            return Ok(new HomeDetails
            {
                Service = Api.ApiBasicConfiguration?.ApiName,
                BuildVersion = Api.ApiSettings?.BuildVersion,
                Environment = EnvironmentUtility.GetCurrentEnvironment(),
                RequestKey = this.RequestKey.Value,
                Application = Api.ApiSettings.Application,
                Domain = Api.ApiSettings.Domain,
                JsonSerializer = Api.ApiSettings.JsonSerializer,
                EnvironmentPrefix = Api.ApiBasicConfiguration.EnvironmentVariablesPrefix,
                TimezoneInfo = new TimezoneInfo(this.HttpContextAccessor)
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

            public JsonSerializerEnum JsonSerializer { get; set; }
            
            public string RequestKey { get; set; }
        
            public TimezoneInfo TimezoneInfo { get; set; }
        }

        public class TimezoneInfo
        {
            public TimezoneInfo(IHttpContextAccessor httpContextAccessor)
            {
                this.CurrentTimezone = DateTimeConverter.GetTimeZoneByAspNetHeader(
                    httpContextAccessor, 
                    Api.ApiSettings.TimezoneHeader).Id;
            }

            public string UtcNow => DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

            public DateTime CurrentNow => DateTime.UtcNow;

            public string DefaultTimezone => Api.ApiSettings.TimezoneDefault.Id;

            public string CurrentTimezone { get; set; }

            public string TimezoneHeader => Api.ApiSettings.TimezoneHeader;
        }
    }
}
