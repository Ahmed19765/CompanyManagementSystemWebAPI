using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam
{
    public class UnassignProjectFromTeamCommandValidator : AbstractValidator<UnassignProjectFromTeamCommand>
    {
        public UnassignProjectFromTeamCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project id is required.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).WithMessage("Team id is required.");
        }
    }
}
