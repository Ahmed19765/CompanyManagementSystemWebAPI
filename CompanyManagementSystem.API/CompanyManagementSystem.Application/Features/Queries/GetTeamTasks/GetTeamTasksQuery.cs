using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamTasks
{
    public class GetTeamTasksQuery : IRequest<GetTeamTasksResponse>
    {
        [JsonIgnore]
        public Guid RequestingUserId { get; set; }

        public int TeamId { get; set; }
    }
}
