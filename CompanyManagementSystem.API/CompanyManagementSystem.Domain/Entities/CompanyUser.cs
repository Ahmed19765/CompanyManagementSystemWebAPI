using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Domain.Entities
{
    public class CompanyUser
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public CompanyRank Rank { get; set; } = CompanyRank.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
