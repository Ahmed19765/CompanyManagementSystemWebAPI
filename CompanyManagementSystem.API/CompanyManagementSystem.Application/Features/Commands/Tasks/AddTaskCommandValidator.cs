using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.Tasks
{
    public class AddTaskCommandValidator : AbstractValidator<AddTaskCommand>
    {
        public AddTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Task description must not exceed 500 characters.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project is required.");

            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("Team is required.");
        }
    }
}
