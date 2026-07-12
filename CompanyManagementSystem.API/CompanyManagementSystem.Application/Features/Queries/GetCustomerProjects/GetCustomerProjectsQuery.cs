using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects
{
    public class GetCustomerProjectsQuery : IRequest<Response<IEnumerable<CustomerProjectDto>>>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }
    }
}
