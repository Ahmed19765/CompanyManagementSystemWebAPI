namespace CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusResponse
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string NewStatus { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
