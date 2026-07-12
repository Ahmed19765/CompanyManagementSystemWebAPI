using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberCommandValidator : AbstractValidator<AddCompanyMemberCommand>
    {
        public AddCompanyMemberCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company id is required.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.");
        }
    }
}
