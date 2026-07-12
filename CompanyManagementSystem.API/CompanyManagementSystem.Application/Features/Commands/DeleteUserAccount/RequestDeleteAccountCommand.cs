using CompanyManagementSystem.Application.Common;
using MediatR;
using System;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class RequestDeleteAccountCommand : IRequest<Response<string>>
    {
        public Guid UserId { get; set; }
    }
}
