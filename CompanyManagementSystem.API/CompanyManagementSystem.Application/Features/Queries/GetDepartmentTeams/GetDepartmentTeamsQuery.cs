using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetDepartmentTeams
{
    public class GetDepartmentTeamsQuery : IRequest<GetDepartmentTeamsResponse>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public int DepartmentId { get; set; }
    }
}
