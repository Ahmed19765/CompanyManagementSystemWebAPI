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

            // Company relationship
            builder.HasOne(d => d.Company)
                   .WithMany(c => c.Departments)
                   .HasForeignKey(d => d.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Teams
            builder.HasMany(d => d.Teams)
                   .WithOne(t => t.Department)
                   .HasForeignKey(t => t.DepartmentId);
        }
    }
}
