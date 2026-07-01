using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class CompanyOffersConfiguration : IEntityTypeConfiguration<CompanyOffers>
    {
        public void Configure(EntityTypeBuilder<CompanyOffers> builder)
        {
            // Composite Key
            builder.HasKey(co => new { co.CompanyId, co.ProjectId });

            builder.Property(co => co.OfferedBudget)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(co => co.StartDate)
                   .IsRequired();

            builder.Property(co => co.DeliveryExceptedDate)
                   .IsRequired();

            builder.Property(co => co.Status)
                   .HasMaxLength(20)
                   .HasConversion<string>();

            // Company relationship
            builder.HasOne(co => co.Company)
                   .WithMany(c => c.CompanyOffers)
                   .HasForeignKey(co => co.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Project relationship
            builder.HasOne(co => co.Project)
                   .WithMany(p => p.CompanyOffers)
                   .HasForeignKey(co => co.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
