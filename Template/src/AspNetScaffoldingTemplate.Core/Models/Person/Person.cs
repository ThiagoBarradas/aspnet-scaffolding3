using AspNetScaffoldingTemplate.Core.Models.Person.Composition;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetScaffoldingTemplate.Core.Models.Person
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime Birthdate { get; set; }

        public DateTime Now { get; set; } = DateTime.UtcNow;

        public string Email { get; set; }

        public Address Address { get; set; }

        public PersonType Type { get; set; }
    }
}
