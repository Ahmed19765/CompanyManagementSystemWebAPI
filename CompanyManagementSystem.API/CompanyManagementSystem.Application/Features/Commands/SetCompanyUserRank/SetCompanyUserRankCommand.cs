using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank
{
    public class SetCompanyUserRankCommand : IRequest<SetCompanyUserRankResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public int CompanyId { get; set; }
        public Guid UserId { get; set; }
        public CompanyRank Rank { get; set; }
    }
}
