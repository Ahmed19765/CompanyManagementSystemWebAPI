namespace CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks
{
    public class MyTaskDto
    {
        public Guid TaskId { get; set; }
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
