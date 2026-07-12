namespace CompanyManagementSystem.Application.Features.Commands.UpdateProject
{
    public class UpdateProjectResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public string? ProjectDocumentsUrl { get; set; }
        public decimal ProjectOfferedBudget { get; set; }
        public string Message { get; set; } = null!;
    }
}
