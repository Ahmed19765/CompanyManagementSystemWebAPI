namespace CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks
{
    public class GetMyAssignedTasksResponse
    {
        public IEnumerable<MyTaskDto> Tasks { get; set; } = new List<MyTaskDto>();
    }

    public class MyTaskDto
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string? TaskDescription { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string? ProjectTitle { get; set; }
        public string? TeamName { get; set; }
        public string? Notes { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
