namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamResponse
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
