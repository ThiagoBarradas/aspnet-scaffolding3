using AspNetScaffolding.DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using WebApi.Models.Response;

namespace AspNetScaffolding.Controllers
{
    public class PersonController : BaseController
    {
        public PersonController()
        {

        }

        [HttpGet("persons/{id}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Birthdate = DateTime.UtcNow.AddMonths(30),
                Email = "john.doe@email.com",
                Type = PersonType.PhysicalPerson
            };

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
