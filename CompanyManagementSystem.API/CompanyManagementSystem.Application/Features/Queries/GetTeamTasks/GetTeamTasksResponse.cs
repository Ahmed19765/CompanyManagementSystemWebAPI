namespace CompanyManagementSystem.Application.Features.Queries.GetTeamTasks
{
    public class GetTeamTasksResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public IEnumerable<TaskSummaryDto> Tasks { get; set; } = new List<TaskSummaryDto>();
    }

    public class TaskSummaryDto
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string? TaskDescription { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string? AssignedToUserName { get; set; }
        public string? AssignedByUserName { get; set; }
        public string? Notes { get; set; }
        public string? AcceptanceCriteria { get; set; }
    }
}
