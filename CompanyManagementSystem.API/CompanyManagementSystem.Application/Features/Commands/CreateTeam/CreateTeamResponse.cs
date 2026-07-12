namespace CompanyManagementSystem.Application.Features.Commands.CreateTeam
{
    public class CreateTeamResponse
    {
        public Guid TeamId { get; set; }
        public string Message { get; set; } = null!;
    }
}
