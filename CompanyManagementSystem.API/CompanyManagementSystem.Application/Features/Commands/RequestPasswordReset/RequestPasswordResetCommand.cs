using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommand : IRequest<RequestPasswordResetResponse>
    {
        public string Email { get; set; } = null!;
    }
}
