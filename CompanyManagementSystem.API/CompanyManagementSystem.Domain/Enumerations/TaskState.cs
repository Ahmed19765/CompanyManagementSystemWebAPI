namespace CompanyManagementSystem.Domain.Enumerations
{
    // Only these values are allowed — no typos, no free text
    public enum TaskState
    {
        Todo,        // Task created but not started yet
        InProgress,  // Engineer is working on it
        Pending,     // Waiting for something (review, info, etc.)
        Done,        // Engineer marked it complete
        Failed       // Task could not be completed
    }
}
