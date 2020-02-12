using AspNetScaffoldingTemplate.Core.Models.Person.Composition;
using FluentValidation;

namespace AspNetScaffoldingTemplate.Core.Models.Person.Validator
{
    public class CreatePersonValidator : AbstractValidator<Person>
    {
        public CreatePersonValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().Length(3, 30);
            RuleFor(p => p.LastName).NotEmpty().Length(3, 30);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Address).NotNull().SetValidator(new AddressValidator());
            RuleFor(p => p.Type).Must(p => p != PersonType.Undefined);
        }
    }
}
