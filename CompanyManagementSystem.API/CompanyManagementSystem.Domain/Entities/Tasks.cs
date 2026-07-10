using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Domain.Entities
{
    public class Tasks
    {
        public int TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }

        // Only valid values allowed — set by Engineer
        public TaskState Status { get; set; } = TaskState.Todo;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }  // deadline for this task

        // Project this task belongs to
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        // Team this task belongs to — nullable so task can survive if team is deleted
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        // Who CREATED / ASSIGNED this task (TeamLead or Owner)
        // Nullable so EF can ClientSetNull when the assigning user is deleted
        public Guid? AssignedById { get; set; }
        public User? AssignedBy { get; set; }

        // Who is DOING this task (Engineer)
        public Guid? AssignedToId { get; set; }   // nullable — task may not be assigned to anyone yet
        public User? AssignedTo { get; set; }

        // Full detail page info (One-to-One)
        public TaskDetails? Details { get; set; }
    }
}
