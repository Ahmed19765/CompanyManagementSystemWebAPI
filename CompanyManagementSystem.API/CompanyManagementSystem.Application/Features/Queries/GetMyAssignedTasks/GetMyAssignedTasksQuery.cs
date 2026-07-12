using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks
{
    public class GetMyAssignedTasksQuery : IRequest<Response<IEnumerable<MyTaskDto>>>
    {
        // Injected from JWT — the engineer requesting their own tasks
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
