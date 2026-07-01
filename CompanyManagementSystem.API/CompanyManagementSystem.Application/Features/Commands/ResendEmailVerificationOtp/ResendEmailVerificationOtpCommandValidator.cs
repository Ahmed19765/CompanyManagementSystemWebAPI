using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.ResendEmailVerificationOtp
{
    public class ResendEmailVerificationOtpCommandValidator : AbstractValidator<ResendEmailVerificationOtpCommand>
    {
        public ResendEmailVerificationOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
