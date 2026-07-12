using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam
{
    public class UnassignProjectFromTeamCommandValidator : AbstractValidator<UnassignProjectFromTeamCommand>
    {
        public UnassignProjectFromTeamCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project id is required.");

            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team id is required.");
        }
    }
}
