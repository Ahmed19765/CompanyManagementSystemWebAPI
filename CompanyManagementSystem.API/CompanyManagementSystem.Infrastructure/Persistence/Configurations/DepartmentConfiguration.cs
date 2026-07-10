using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(d => d.DepartmentDescription)
                   .HasMaxLength(500);

            // ClientCascade: EF handles delete in memory to avoid multiple cascade path conflict.
            // Path exists: AspNetUsers → Companies (cascade) → Departments
            // A second DB-level cascade from Companies → Departments causes SQL Server error 1785.
            builder.HasOne(d => d.Company)
                   .WithMany(c => c.Departments)
                   .HasForeignKey(d => d.CompanyId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            // Teams — ClientSetNull: deleting a department nulls Team.DepartmentId.
            // Teams survive and can be reassigned to another department later.
            builder.HasMany(d => d.Teams)
                   .WithOne(t => t.Department)
                   .HasForeignKey(t => t.DepartmentId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
