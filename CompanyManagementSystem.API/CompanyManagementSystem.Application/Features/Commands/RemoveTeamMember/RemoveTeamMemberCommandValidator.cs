using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberCommandValidator : AbstractValidator<RemoveTeamMemberCommand>
    {
        public RemoveTeamMemberCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required.");
        }
    }
}
