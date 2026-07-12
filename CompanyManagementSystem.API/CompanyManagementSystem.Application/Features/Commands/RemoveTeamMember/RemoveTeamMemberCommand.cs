using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberCommand : IRequest<Response<string>>
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
    }
}
