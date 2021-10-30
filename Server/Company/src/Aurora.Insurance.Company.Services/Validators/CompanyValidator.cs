using Aurora.Core.Data;
using FluentValidation;

namespace Aurora.Insurance.Company.Services.Validators
{
    public class CompanyValidator : AbstractValidator<Domain.Models.Entities.Company>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.Id).NotEmpty().MaximumLength(StandardStringLengths.Code);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(StandardStringLengths.DefaultString);
        }
    }
}
