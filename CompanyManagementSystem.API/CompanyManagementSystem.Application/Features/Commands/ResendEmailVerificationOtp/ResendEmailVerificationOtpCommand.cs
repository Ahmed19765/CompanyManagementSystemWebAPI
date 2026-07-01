using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.ResendEmailVerificationOtp
{
    public class ResendEmailVerificationOtpCommand : IRequest<ResendEmailVerificationOtpResponse>
    {
        public string Email { get; set; } = null!;
    }
}
