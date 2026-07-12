using CompanyManagementSystem.Application.Common;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
