using Microsoft.AspNetCore.Mvc;
using System;

namespace AspNetScaffolding.DemoApi.Models
{
    public class CustomerRequest
    {
        [FromRoute] public string CustomerId { get; set; }

        [FromQuery] public string ServiceId { get; set; }
    }

    public class CustomerRequest2
    {
        [FromRoute] public string CustomerId { get; set; }

        [FromBody] public string OtherProp { get; set; }

        [FromBody] public DateTime DateTest { get; set; }
    }
}
