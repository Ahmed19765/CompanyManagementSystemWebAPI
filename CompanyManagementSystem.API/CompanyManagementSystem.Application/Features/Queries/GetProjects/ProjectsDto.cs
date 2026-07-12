namespace CompanyManagementSystem.Application.Features.Queries.GetProjects
{
    public class ProjectsDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public decimal OfferedBudget { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? ProjectState { get; set; }
    }
}
