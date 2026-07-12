using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyAcceptedProjects
{
    public class GetCompanyAcceptedProjectsQuery : IRequest<Response<GetCompanyAcceptedProjectsResponse>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client — which of the owner's companies to check
        public Guid CompanyId { get; set; }
    }
}
