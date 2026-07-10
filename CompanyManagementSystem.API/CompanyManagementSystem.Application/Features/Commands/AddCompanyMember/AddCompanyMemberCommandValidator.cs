using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberCommandValidator : AbstractValidator<AddCompanyMemberCommand>
    {
        public AddCompanyMemberCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId must be a positive integer.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.");
        }
    }
}
