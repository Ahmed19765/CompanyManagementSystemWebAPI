using CompanyManagementSystem.Application.Common;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.Tasks
{
    public class AddTaskCommand : IRequest<Response<AddTaskResponse>>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public Guid TeamId { get; set; }
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
