using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyMembers
{
    public class GetCompanyMembersQuery : IRequest<GetCompanyMembersResponse>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public int CompanyId { get; set; }
    }
}
