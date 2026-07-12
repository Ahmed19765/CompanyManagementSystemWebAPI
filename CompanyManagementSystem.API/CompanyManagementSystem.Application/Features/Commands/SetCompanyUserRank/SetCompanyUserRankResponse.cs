using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank
{
    public class SetCompanyUserRankResponse
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public CompanyRank Rank { get; set; }
        public string Message { get; set; } = null!;
    }
}
