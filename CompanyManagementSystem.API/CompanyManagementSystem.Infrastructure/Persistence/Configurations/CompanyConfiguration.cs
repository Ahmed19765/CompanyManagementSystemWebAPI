using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.CompanyId);

            builder.Property(c => c.CompanyName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.CompanyDescription)
                   .HasMaxLength(500);

            builder.Property(c => c.JoinCode)
                   .HasMaxLength(36)
                   .IsRequired();

            builder.Property(c => c.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(c => c.DeletedAt);

            //Company Owners (1-to-Many)
            builder.HasOne(c => c.Owner)
                   .WithMany(u => u.OwnedCompanies)
                   .HasForeignKey(c => c.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Departments (1-to-Many)
            builder.HasMany(c => c.Departments)
                   .WithOne(d => d.Company)
                   .HasForeignKey(d => d.CompanyId);

            builder.HasMany(c => c.CompanyUsers)
                   .WithOne(cu => cu.Company)
                   .HasForeignKey(cu => cu.CompanyId);

            builder.HasIndex(c => c.CompanyName)
                   .IsUnique();
        }
    }
}
