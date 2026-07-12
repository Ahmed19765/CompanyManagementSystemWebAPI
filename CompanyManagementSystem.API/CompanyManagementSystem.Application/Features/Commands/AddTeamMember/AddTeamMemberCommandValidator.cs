using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AddTeamMember
{
    public class AddTeamMemberCommandValidator : AbstractValidator<AddTeamMemberCommand>
    {
        public AddTeamMemberCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required.");
        }
    }
}
