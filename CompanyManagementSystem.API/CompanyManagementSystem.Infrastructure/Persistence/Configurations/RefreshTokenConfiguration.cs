using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.RefreshTokenId); 

            builder.Property(rt => rt.Token)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                   .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                   .IsRequired();

            builder.Property(rt => rt.IsUsed)
                   .IsRequired();

            // IsExpired and IsActive computed properties — EF 
            builder.Ignore(rt => rt.IsExpired);
            builder.Ignore(rt => rt.IsActive);

            // User relationship
            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshToken)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
