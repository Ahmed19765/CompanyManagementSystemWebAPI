using CompanyManagementSystem.Application.Common;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class DeleteAccountCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string Otp { get; set; } = null!;
    }
}
