using CompanyManagementSystem.Application.Common;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.ResendEmailVerificationOtp
{
    public class ResendEmailVerificationOtpCommand : IRequest<Response<string>>
    {
        public string Email { get; set; } = null!;
    }
}
