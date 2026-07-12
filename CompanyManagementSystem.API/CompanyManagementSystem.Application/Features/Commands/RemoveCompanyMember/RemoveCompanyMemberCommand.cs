using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveCompanyMember
{
    public class RemoveCompanyMemberCommand : IRequest<Response<RemoveCompanyMemberResponse>>
    {
        // Injected by the controller from JWT — never from request body
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // From the route: DELETE /api/company/{companyId}/members/{userId}
        public Guid CompanyId { get; set; }

        public Guid TargetUserId { get; set; }
    }
}
