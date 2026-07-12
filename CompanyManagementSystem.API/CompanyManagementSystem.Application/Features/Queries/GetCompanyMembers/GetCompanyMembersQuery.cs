using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyMembers
{
    public class GetCompanyMembersQuery : IRequest<Response<GetCompanyMembersResponse>>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public Guid CompanyId { get; set; }
    }
}
