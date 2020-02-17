using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Utilities;
using AspNetScaffolding3.Extensions.GracefullShutdown;
using AspNetSerilog.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackUtils.Converters;
using System;
using System.Threading;

namespace AspNetScaffolding.Controllers
{
    [ApiController]
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
            this.HttpContextAccessor = httpContextAccessor;
            this.GracefullShutdownState = gracefullShutdownState;
            this.RequestKey = requestKey;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(HomeDetails), 200)]
        public IActionResult Home()
        {
            this.DisableLogging();
            
            return Ok(new HomeDetails
            {
                RIP = this.GracefullShutdownState.RequestsInProgress,
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

        [HttpGet("delay")]
        [ProducesResponseType(typeof(HomeDetails), 200)]
        public IActionResult Delay()
        {
            Thread.Sleep(20000);

            return Ok();
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
                this.CurrentTimezone = DateTimeConverter.GetTimeZoneByAspNetHeader(
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
