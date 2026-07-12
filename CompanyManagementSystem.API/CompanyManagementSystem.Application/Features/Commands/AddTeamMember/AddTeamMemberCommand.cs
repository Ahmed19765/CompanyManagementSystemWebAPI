using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AddTeamMember
{
    public class AddTeamMemberCommand : IRequest<Response<string>>
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public string? TeamRole { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
    }
}
