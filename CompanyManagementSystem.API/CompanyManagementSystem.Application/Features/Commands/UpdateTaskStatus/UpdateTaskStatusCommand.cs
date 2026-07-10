using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand : IRequest<UpdateTaskStatusResponse>
    {
        // Injected from JWT
        [JsonIgnore]
        public Guid UserId { get; set; }

        // Sent from the client in the JSON body
        public int TaskId { get; set; }
        public TaskState NewStatus { get; set; }
    }
}
