using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
    {
        public void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            builder.HasKey(cu => new { cu.CompanyId, cu.UserId });

            builder.Property(cu => cu.Rank)
                   .HasMaxLength(50)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(cu => cu.JoinedAt)
                   .IsRequired();

            // Delete Company → delete its CompanyUser rows (single cascade path ✅)
            builder.HasOne(cu => cu.Company)
                   .WithMany(c => c.CompanyUsers)
                   .HasForeignKey(cu => cu.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Delete User → EF handles nulling/removing CompanyUser rows in memory
            // Avoids multiple cascade path conflict:
            //   AspNetUsers → Companies (cascade) → CompanyUsers
            //   AspNetUsers →                       CompanyUsers  ← second path, SQL Server rejects
            builder.HasOne(cu => cu.User)
                   .WithMany(u => u.CompanyMemberships)
                   .HasForeignKey(cu => cu.UserId)
                   .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
