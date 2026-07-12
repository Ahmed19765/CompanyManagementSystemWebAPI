using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamCommandValidator : AbstractValidator<AssignProjectToTeamCommand>
    {
        public AssignProjectToTeamCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project id is required.");

            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team id is required.");
        }
    }
}
