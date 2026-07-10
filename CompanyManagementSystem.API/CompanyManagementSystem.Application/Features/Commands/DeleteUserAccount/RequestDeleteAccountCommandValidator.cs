using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class RequestDeleteAccountCommandValidator : AbstractValidator<RequestDeleteAccountCommand>
    {
        public RequestDeleteAccountCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
