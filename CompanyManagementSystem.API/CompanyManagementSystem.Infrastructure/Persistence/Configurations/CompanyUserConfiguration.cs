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

            builder.HasOne(cu => cu.Company)
                   .WithMany(c => c.CompanyUsers)
                   .HasForeignKey(cu => cu.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cu => cu.User)
                   .WithMany(u => u.CompanyMemberships)
                   .HasForeignKey(cu => cu.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
