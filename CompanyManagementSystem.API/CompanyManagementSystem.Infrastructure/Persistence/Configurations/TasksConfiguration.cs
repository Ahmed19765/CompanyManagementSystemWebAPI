using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(t => t.TaskId);

            builder.Property(t => t.TaskTitle)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(t => t.TaskDescription)
                   .HasMaxLength(500);

            builder.Property(t => t.Status)
                   .HasConversion<string>()
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(t => t.AssignedById)
                   .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            builder.Property(t => t.DueDate);

            // Project relationship
            builder.HasOne(t => t.Project)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Team relationship
            builder.HasOne(t => t.Team)
                   .WithMany(tm => tm.Tasks)
                   .HasForeignKey(t => t.TeamId)
                   .OnDelete(DeleteBehavior.SetNull);

            // AssignedBy (User) — ClientSetNull avoids multiple cascade path conflict.
            // When user is deleted, AssignedById becomes null (task record is kept).
            builder.HasOne(t => t.AssignedBy)
                   .WithMany(u => u.AssignedByMe)
                   .HasForeignKey(t => t.AssignedById)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            // AssignedTo (User)
            builder.HasOne(t => t.AssignedTo)
                   .WithMany(u => u.AssignedToMe)
                   .HasForeignKey(t => t.AssignedToId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            // TaskDetails (One-to-One)
            builder.HasOne(t => t.Details)
                   .WithOne(td => td.Task)
                   .HasForeignKey<TaskDetails>(td => td.TaskId);
        }
    }
}
