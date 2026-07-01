using System;

namespace CompanyManagementSystem.Application.Features.Commands.Tasks
{
    public class AddTaskResponse
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
