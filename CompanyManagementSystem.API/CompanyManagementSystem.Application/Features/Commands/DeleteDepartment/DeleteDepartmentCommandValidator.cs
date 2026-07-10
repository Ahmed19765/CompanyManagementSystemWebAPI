using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
    {
        public DeleteDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department id is required.");
        }
    }
}
