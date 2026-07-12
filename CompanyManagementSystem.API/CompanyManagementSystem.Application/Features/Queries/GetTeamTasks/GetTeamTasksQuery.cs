using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamTasks
{
    public class GetTeamTasksQuery : IRequest<Response<GetTeamTasksResponse>>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public Guid TeamId { get; set; }
    }
}
