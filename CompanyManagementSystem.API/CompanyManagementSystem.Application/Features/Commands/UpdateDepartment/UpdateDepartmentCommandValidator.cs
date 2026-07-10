using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department id is required.");

            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name is too long.");

            RuleFor(x => x.DepartmentDescription)
                .MaximumLength(500).WithMessage("Department description is too long.");
        }
    }
}
