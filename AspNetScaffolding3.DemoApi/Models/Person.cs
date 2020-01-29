using FluentValidation;
using Newtonsoft.Json;
using PackUtils.Converters;
using System;

namespace AspNetScaffolding.DemoApi.Models
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

    public enum PersonType
    {
        Undefined,
        PhysicalPerson,
        LegalEntity
    }

    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().Length(3, 30);
            RuleFor(p => p.LastName).NotEmpty().Length(3, 30);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Address).NotNull().SetValidator(new AddressValidator());
            RuleFor(p => p.Type).Must(p => p != PersonType.Undefined);
        }
    }
}
