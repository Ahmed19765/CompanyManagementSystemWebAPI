using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyAcceptedProjects
{
    public class GetCompanyAcceptedProjectsQuery : IRequest<GetCompanyAcceptedProjectsResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client — which of the owner's companies to check
        public int CompanyId { get; set; }
    }
}
