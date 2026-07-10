using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteCompany
{
    public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
    {
        public DeleteCompanyCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company id is required.");
        }
    }
}
