namespace CompanyManagementSystem.Application.Features.Commands.CreateProject
{
    public class CreateProjectResponse
    {
        public Guid ProjectId { get; set; }
        public string Message { get; set; } = null!;
    }
}
