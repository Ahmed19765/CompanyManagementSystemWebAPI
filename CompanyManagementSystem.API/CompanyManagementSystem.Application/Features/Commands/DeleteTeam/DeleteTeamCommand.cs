using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteTeam
{
    public class DeleteTeamCommand : IRequest<Response<string>>
    {
        public Guid TeamId { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
    }
}
