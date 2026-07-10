using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferCommandValidator : AbstractValidator<AcceptProjectOfferCommand>
    {
        public AcceptProjectOfferCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project id is required.");

            RuleFor(x => x.ChosenCompanyId)
                .GreaterThan(0).WithMessage("Chosen company id is required.");
        }
    }
}
