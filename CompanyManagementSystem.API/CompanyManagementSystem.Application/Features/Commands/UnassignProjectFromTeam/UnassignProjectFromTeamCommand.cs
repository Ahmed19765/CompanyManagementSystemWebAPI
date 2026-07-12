using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam
{
    public class UnassignProjectFromTeamCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public Guid ProjectId { get; set; }
        public Guid TeamId { get; set; }
    }
}
