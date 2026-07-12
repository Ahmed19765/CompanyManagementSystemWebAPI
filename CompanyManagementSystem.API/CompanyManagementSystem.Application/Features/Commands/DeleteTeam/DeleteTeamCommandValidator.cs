using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteTeam
{
    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team id is required.");
        }
    }
}
