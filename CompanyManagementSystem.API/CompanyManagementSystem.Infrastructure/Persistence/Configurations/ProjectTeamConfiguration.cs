using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
    {
        public void Configure(EntityTypeBuilder<ProjectTeam> builder)
        {
            // Composite Key
            builder.HasKey(pt => new { pt.ProjectId, pt.TeamId });

            // Project relationship — delete project → delete its team assignments
            // ClientCascade: EF handles it in memory to avoid multiple cascade path conflicts
            builder.HasOne(pt => pt.Project)
                   .WithMany(p => p.AssignedTeams)
                   .HasForeignKey(pt => pt.ProjectId)
                   .OnDelete(DeleteBehavior.ClientCascade);

            // Team relationship
            builder.HasOne(pt => pt.Team)
                   .WithMany(t => t.AssignedProjects)
                   .HasForeignKey(pt => pt.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pt => pt.AssignedAt)
                   .IsRequired();
        }
    }
}
