using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam
{
    public class UnassignProjectFromTeamCommand : IRequest<UnassignProjectFromTeamResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public int ProjectId { get; set; }
        public int TeamId { get; set; }
    }
}
