namespace CompanyManagementSystem.Domain.Entities
{
    // One-to-One with Tasks
    // Shown on the Task detail page
    public class TaskDetails
    {
        public int TaskDetailsId { get; set; }

        // Link back to the Task (One-to-One)
        public int TaskId { get; set; }
        public Tasks? Task { get; set; }

        public string? Notes { get; set; }             // extra notes written by TeamLead
        public string? TaskAttachmentDocumentUrl { get; set; }
        public string? AcceptanceCriteria { get; set; } // what "Done" looks like for this task or what expected to receive from the Engineer
    }
}
