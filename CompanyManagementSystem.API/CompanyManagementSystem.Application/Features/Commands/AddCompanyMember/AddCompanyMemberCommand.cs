using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberCommand : IRequest<Response<AddCompanyMemberResponse>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public Guid CompanyId { get; set; }
        public string UserName { get; set; } = null!;
    }
}
