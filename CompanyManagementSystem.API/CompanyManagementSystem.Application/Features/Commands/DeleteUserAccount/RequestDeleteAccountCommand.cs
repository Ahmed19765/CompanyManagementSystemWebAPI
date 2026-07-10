using MediatR;
using System;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class RequestDeleteAccountCommand : IRequest<RequestDeleteAccountResponse>
    {
        public Guid UserId { get; set; }
    }
}
