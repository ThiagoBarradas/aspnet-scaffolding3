using AspNetScaffolding.DemoApi.Models;
using AspNetScaffolding.Extensions.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mundipagg;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApi.Models.Exceptions;
using WebApi.Models.Response;

namespace AspNetScaffolding.Controllers
{
    public class PersonController : BaseController
    {
        private IDistributedCache DistributedCache { get; set; }


        private ILocker Locker { get; set; }

        public PersonController(IDistributedCache cache, ILocker locker, IMundipaggApiClient client)
        {
            this.Locker = locker;
            this.DistributedCache = cache;
        }

        [HttpGet("persons/{id}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Birthdate = DateTime.UtcNow.AddMonths(30),
                Email = "john.doe@email.com",
                Type = PersonType.PhysicalPerson
            };

            return Ok(person);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await this.DistributedCache.SetStringAsync("123123", "teste", options);

            var locker = await this.Locker.GetDistributedLockerAsync("teste", 600);

            if (!locker.IsAcquired)
            {
                throw new ConflictException();
            }

            var apiResponse = new ApiResponse
            {
                Content = person,
                StatusCode = HttpStatusCode.Accepted,
                Headers = new Dictionary<string, string>
                {
                    { "SomeHeader", "Test Get" }
                }
            };

            return CreateJsonResponse(apiResponse);
        }

        [HttpPost("persons")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Create([FromBody]Person person)
        {
            Validate(person);

            var apiResponse = new ApiResponse
            {
                Content = person,
                StatusCode = HttpStatusCode.Created,
                Headers = new Dictionary<string, string>
                {
                    { "SomeHeader", "Test Created" }
                }
            };

            return CreateJsonResponse(apiResponse);
        }
    }
}
