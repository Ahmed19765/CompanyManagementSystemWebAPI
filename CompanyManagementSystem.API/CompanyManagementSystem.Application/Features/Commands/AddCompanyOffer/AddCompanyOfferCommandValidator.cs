using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer
{
    public class AddCompanyOfferCommandValidator : AbstractValidator<AddCompanyOfferCommand>
    {
        public AddCompanyOfferCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company id is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project id is required.");

            RuleFor(x => x.OfferedBudget)
                .GreaterThan(0).WithMessage("Offered budget must be greater than zero.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.DeliveryExpectedDate)
                .NotEmpty().WithMessage("Delivery date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("Delivery date must be after the start date.");
        }
    }
}
