using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamCommandValidator : AbstractValidator<AssignProjectToTeamCommand>
    {
        public AssignProjectToTeamCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project id is required.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).WithMessage("Team id is required.");
        }
    }
}
