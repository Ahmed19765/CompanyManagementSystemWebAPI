using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveCompanyMember
{
    public class RemoveCompanyMemberCommandValidator : AbstractValidator<RemoveCompanyMemberCommand>
    {
        public RemoveCompanyMemberCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company id is required.");

            RuleFor(x => x.TargetUserId)
                .NotEmpty().WithMessage("Target user id is required.");
        }
    }
}
