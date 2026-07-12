using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferCommandValidator : AbstractValidator<AcceptProjectOfferCommand>
    {
        public AcceptProjectOfferCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project id is required.");

            RuleFor(x => x.ChosenCompanyId)
                .NotEmpty().WithMessage("Chosen company id is required.");
        }
    }
}
