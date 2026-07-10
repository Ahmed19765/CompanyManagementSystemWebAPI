using MediatR;
using System;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.Tasks
{
    public class AddTaskCommand : IRequest<AddTaskResponse>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public int TeamId { get; set; }
        public Guid? AssignedToId { get; set; }

        // Task Details fields
        public string? Notes { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? AcceptanceCriteria { get; set; }

        [JsonIgnore]
        // Injected from JWT Claims in Controller
        public Guid CurrentUserId { get; set; }
    }
}
