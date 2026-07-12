using CompanyManagementSystem.Application.Common;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Response<RefreshTokenResponse>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
