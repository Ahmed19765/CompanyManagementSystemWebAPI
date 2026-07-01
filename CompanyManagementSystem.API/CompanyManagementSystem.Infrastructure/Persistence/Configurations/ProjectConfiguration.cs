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

            builder.Property(p => p.UploadedDate);

            // Customer (User)
            builder.HasOne(p => p.Customer)
                   .WithMany(u => u.OwnedProjects)
                   .HasForeignKey(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Assigned Teams (Many-to-Many)
            builder.HasMany(p => p.AssignedTeams)
                   .WithOne(pt => pt.Project)
                   .HasForeignKey(pt => pt.ProjectId);

            // Company Offers
            builder.HasMany(p => p.CompanyOffers)
                   .WithOne(co => co.Project)
                   .HasForeignKey(co => co.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
