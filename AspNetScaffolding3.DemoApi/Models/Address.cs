using FluentValidation;

namespace AspNetScaffolding.DemoApi.Models
{
    public class Address
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }       

        public string CityCode { get; set; }
    }

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(p => p.Line1).NotEmpty().Length(3, 30);
            RuleFor(p => p.Line2).NotEmpty().Length(3, 30);
            RuleFor(p => p.CityCode).NotEmpty().Length(2);
        }
    }
}
