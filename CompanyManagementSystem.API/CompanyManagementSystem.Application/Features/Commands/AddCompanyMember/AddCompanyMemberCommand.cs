using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberCommand : IRequest<AddCompanyMemberResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public int CompanyId { get; set; }
        public string UserName { get; set; } = null!;
    }
}
