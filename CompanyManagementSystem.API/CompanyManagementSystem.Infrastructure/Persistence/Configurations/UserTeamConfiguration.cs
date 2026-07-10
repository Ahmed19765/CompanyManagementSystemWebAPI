using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.TeamId });
            builder.Property(ut => ut.JoinedAt)
                   .IsRequired();
            builder.Property(ut => ut.TeamRole)
                   .HasMaxLength(100);

            // Delete User → EF handles removing UserTeam rows in memory
            // Avoids multiple cascade path conflict:
            //   AspNetUsers → Companies → Departments → Teams → UserTeams
            //   AspNetUsers →                                    UserTeams  ← second path
            builder.HasOne(ut => ut.User)
                   .WithMany(u => u.UserTeams)
                   .HasForeignKey(ut => ut.UserId)
                   .OnDelete(DeleteBehavior.ClientCascade);

            // Delete Team → delete its UserTeam rows (single cascade path ✅)
            builder.HasOne(ut => ut.Team)
                   .WithMany(t => t.UserTeams)
                   .HasForeignKey(ut => ut.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
