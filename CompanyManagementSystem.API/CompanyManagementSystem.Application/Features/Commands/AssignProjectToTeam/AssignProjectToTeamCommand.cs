using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamCommand : IRequest<Response<AssignProjectToTeamResponse>>
    {
        // Injected from JWT
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public Guid ProjectId { get; set; }
        public Guid TeamId { get; set; }
    }
}
