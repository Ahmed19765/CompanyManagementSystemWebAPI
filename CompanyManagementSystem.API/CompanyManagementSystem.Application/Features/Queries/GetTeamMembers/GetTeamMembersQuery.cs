using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamMembers
{
    public class GetTeamMembersQuery : IRequest<Response<GetTeamMembersResponse>>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public Guid TeamId { get; set; }
    }
}
