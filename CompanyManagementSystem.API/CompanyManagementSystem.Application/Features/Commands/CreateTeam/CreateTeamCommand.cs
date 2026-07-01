using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.CreateTeam
{
    public class CreateTeamCommand : IRequest<CreateTeamResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public int DepartmentId { get; set; }

        public string? LeaderUserName { get; set; }
        public string TeamName { get; set; } = null!;
        public string? TeamDescription { get; set; }
    }
}
