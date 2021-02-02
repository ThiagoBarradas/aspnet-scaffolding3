using AspNetScaffolding.Controllers;
using AspNetScaffolding3.Extensions.RoutePrefix;
using Microsoft.AspNetCore.Mvc;

namespace AspNetScaffolding3.DemoApi.Controllers
{
    [IgnoreRoutePrefix]
    public class ProxyController : BaseController
    {
        [HttpGet, HttpPost, HttpPut, HttpPatch, HttpDelete, HttpHead, HttpOptions]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        [ProducesResponseType(412)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult HandleAllRequests()
        {
            return Ok();
        }
    }
}
