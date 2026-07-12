using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.CreateTeam
{
    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department id is required.");

            RuleFor(x => x.LeaderUserName)
                .NotEmpty().WithMessage("At least one leader is required to be added for a team.");

            RuleFor(x => x.TeamName)
                .NotEmpty().WithMessage("Team name is required.")
                .MaximumLength(100).WithMessage("Team name is too long.");

            RuleFor(x => x.TeamDescription)
                .MaximumLength(500).WithMessage("Team description is too long.");
        }
    }
}
