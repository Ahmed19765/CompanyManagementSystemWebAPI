using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.TeamId);

            builder.Property(t => t.TeamName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(t => t.TeamDescription)
                   .HasMaxLength(500);

            // Team Members
            builder.HasMany(t => t.UserTeams)
                   .WithOne(ut => ut.Team)
                   .HasForeignKey(ut => ut.TeamId);

            // Team Leader
            builder.HasOne(t => t.Leader)
                   .WithMany(u => u.LeadingTeams)
                   .HasForeignKey(t => t.LeaderId);

            // Department
            builder.HasOne(t => t.Department)
                   .WithMany(d => d.Teams)
                   .HasForeignKey(t => t.DepartmentId);
        }

    }
}
