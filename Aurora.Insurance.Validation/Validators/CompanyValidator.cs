using Aurora.Insurance.EFModel;
using FluentValidation;

namespace Aurora.Insurance.Validation.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.Id).NotEmpty().MaximumLength(StandardStringLengths.Code);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(StandardStringLengths.DefaultString);
        }
    }
}