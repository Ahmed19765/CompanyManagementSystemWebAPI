using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetOwnerCompanies
{
    public class GetOwnerCompaniesQuery : IRequest<Response<IEnumerable<CompanyDto>>>
    {
        // Injected by the controller from the JWT claim — not from the request body
        [JsonIgnore]
        public Guid OwnerId { get; set; }
    }
}
