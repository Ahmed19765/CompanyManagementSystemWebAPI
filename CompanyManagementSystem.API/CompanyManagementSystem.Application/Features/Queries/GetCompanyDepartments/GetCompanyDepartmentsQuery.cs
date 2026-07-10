using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyDepartments
{
    public class GetCompanyDepartmentsQuery : IRequest<GetCompanyDepartmentsResponse>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public int CompanyId { get; set; }
    }
}
