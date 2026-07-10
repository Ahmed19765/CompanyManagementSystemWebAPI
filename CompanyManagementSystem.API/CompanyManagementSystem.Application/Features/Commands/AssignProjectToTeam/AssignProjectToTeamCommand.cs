using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamCommand : IRequest<AssignProjectToTeamResponse>
    {
        // Injected from JWT
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public int ProjectId { get; set; }
        public int TeamId { get; set; }
    }
}
