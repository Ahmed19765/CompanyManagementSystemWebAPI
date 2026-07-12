using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetDepartmentTeams
{
    public class GetDepartmentTeamsQuery : IRequest<Response<GetDepartmentTeamsResponse>>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
