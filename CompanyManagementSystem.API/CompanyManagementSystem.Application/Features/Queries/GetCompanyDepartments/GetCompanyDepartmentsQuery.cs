using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyDepartments
{
    public class GetCompanyDepartmentsQuery : IRequest<Response<GetCompanyDepartmentsResponse>>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public Guid CompanyId { get; set; }
    }
}
