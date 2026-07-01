using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class TaskDetailsConfiguration : IEntityTypeConfiguration<TaskDetails>
    {
        public void Configure(EntityTypeBuilder<TaskDetails> builder)
        {
            builder.HasKey(td => td.TaskDetailsId);

            builder.Property(td => td.Notes)
                   .HasMaxLength(3000);

            builder.Property(td => td.TaskAttachmentDocumentUrl)
                   .HasMaxLength(500);

            builder.Property(td => td.AcceptanceCriteria)
                   .HasMaxLength(2000);

            // One-to-One with Tasks
            builder.HasOne(td => td.Task)
                   .WithOne(t => t.Details)
                   .HasForeignKey<TaskDetails>(td => td.TaskId)
                   .OnDelete(DeleteBehavior.Cascade); // TaskDetails will be deleted if the associated Task is deleted
        }
    }
}
