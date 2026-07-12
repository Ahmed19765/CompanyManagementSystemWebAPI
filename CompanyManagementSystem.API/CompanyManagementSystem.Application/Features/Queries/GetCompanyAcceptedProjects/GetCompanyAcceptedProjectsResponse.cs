namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyAcceptedProjects
{
    public class GetCompanyAcceptedProjectsResponse
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public IEnumerable<AcceptedProjectDto> Projects { get; set; } = new List<AcceptedProjectDto>();
    }

    public class AcceptedProjectDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public string ProjectStatus { get; set; } = null!;

        // Progress: Done tasks / Total tasks * 100
        // Returns 0 if no tasks exist yet
        public int ProgressPercent { get; set; }
        public int TotalTasks { get; set; }
        public int DoneTasks { get; set; }
    }
}
