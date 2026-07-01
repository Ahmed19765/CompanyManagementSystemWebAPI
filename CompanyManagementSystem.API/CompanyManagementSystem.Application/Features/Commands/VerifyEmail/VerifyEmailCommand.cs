using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<VerifyEmailResponse>
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
