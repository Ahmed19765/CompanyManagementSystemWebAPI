using CompanyManagementSystem.Domain.Enumerations;
using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank
{
    public class SetCompanyUserRankCommandValidator : AbstractValidator<SetCompanyUserRankCommand>
    {
        public SetCompanyUserRankCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Company id is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required.");

            RuleFor(x => x.Rank)
                .IsInEnum().WithMessage("Invalid company rank.");
        }
    }
}
