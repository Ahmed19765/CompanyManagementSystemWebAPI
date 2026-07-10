using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.ProjectId);

            builder.Property(p => p.ProjectTitle)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.ProjectDescription)
                   .HasMaxLength(500);

            builder.Property(p => p.ProjectDocumentsUrl)
                   .HasMaxLength(500);

            builder.Property(p => p.ProjectOfferedBudget)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.ProjectStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(p => p.UploadedDate);

            // Customer (User)
            builder.HasOne(p => p.Customer)
                   .WithMany(u => u.OwnedProjects)
                   .HasForeignKey(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Assigned Teams (Many-to-Many)
            builder.HasMany(p => p.AssignedTeams)
                   .WithOne(pt => pt.Project)
                   .HasForeignKey(pt => pt.ProjectId);

            // Company Offers — ClientCascade: ProjectId is part of composite PK, can't be nulled.
            // Deleting a project removes its offers via EF in memory.
            builder.HasMany(p => p.CompanyOffers)
                   .WithOne(co => co.Project)
                   .HasForeignKey(co => co.ProjectId)
                   .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
