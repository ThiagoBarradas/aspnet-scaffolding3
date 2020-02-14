using $ext_safeprojectname$.Models.Person.Composition;
using FluentValidation;

namespace $ext_safeprojectname$.Models.Person.Validator
{
    public class CreatePersonValidator : AbstractValidator<Person>
    {
        public CreatePersonValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().Length(3, 30);
            RuleFor(p => p.LastName).NotEmpty().Length(3, 30);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Type).Must(p => p != PersonType.Undefined);
        }
    }
}
