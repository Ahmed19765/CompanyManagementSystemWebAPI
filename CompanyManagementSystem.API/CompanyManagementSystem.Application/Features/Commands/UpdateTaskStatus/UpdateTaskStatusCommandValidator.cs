using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task id is required.");

            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid task status.");
        }
    }
}
