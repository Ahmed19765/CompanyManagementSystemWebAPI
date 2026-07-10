using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects
{
    public class GetCustomerProjectsQuery : IRequest<GetCustomerProjectsResponse>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }
    }
}
