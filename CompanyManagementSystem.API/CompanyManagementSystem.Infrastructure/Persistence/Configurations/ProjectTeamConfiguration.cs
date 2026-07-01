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

            // Project relationship
            builder.HasOne(pt => pt.Project)
                   .WithMany(p => p.AssignedTeams)
                   .HasForeignKey(pt => pt.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Team relationship
            builder.HasOne(pt => pt.Team)
                   .WithMany(t => t.AssignedProjects)
                   .HasForeignKey(pt => pt.TeamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pt => pt.AssignedAt)
                   .IsRequired();
        }
    }
}
