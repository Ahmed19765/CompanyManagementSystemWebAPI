namespace CompanyManagementSystem.Domain.Entities
{
    // One-to-One with Tasks
    // Shown on the Task detail page
    public class TaskDetails
    {
        public Guid TaskDetailsId { get; set; } = Guid.NewGuid();

        // Link back to the Task (One-to-One)
        public Guid TaskId { get; set; }
        public Tasks? Task { get; set; }

        public string? Notes { get; set; }             // extra notes written by TeamLead
        public string? TaskAttachmentDocumentUrl { get; set; }
        public string? AcceptanceCriteria { get; set; } // what "Done" looks like for this task or what expected to receive from the Engineer
    }
}
