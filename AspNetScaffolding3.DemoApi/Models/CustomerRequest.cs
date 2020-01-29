using Microsoft.AspNetCore.Mvc;

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

        public string OtherProp { get; set; }
    }
}
