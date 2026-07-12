using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand : IRequest<Response<UpdateTaskStatusResponse>>
    {
        // Injected from JWT
        [JsonIgnore]
        public Guid UserId { get; set; }

        // Sent from the client in the JSON body
        public Guid TaskId { get; set; }
        public TaskState NewStatus { get; set; }
    }
}
