using CompanyManagementSystem.Application.Common;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommand : IRequest<Response<string>>
    {
        public string Email { get; set; } = null!;
    }
}
