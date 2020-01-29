using AspNetScaffolding.DemoApi.Entities;
using AspNetScaffolding.DemoApi.Models;

namespace AspNetScaffolding.DemoApi.Profiles
{
    public class CustomerProfile:AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerRequest2, Customer>();
        }
    }
}