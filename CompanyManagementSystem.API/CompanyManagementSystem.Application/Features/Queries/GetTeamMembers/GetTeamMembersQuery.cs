using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamMembers
{
    public class GetTeamMembersQuery : IRequest<GetTeamMembersResponse>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public int TeamId { get; set; }
    }
}
