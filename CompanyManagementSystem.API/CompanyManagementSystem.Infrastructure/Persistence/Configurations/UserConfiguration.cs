using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagementSystem.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Identity already configures: Id (PK), UserName, NormalizedUserName (unique index),
            // Email, NormalizedEmail (unique index), EmailConfirmed, PasswordHash,
            // SecurityStamp, ConcurrencyStamp, PhoneNumber, TwoFactorEnabled,
            // LockoutEnd, LockoutEnabled, AccessFailedCount.
            // Do NOT redefine those — it causes EF conflicts.

            // Custom domain columns only
            builder.Property(u => u.FirstName)
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .HasMaxLength(100);

            builder.Property(u => u.Role)
                   .HasMaxLength(50)
                   .HasConversion<string>();

            builder.Property(u => u.IsBanned)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            // Relationships — unchanged, FK columns are still Guid pointing to User.Id
            builder.HasMany(u => u.LeadingTeams)
                   .WithOne(t => t.Leader)
                   .HasForeignKey(t => t.LeaderId);

            builder.HasMany(u => u.UserTeams)
                   .WithOne(ut => ut.User)
                   .HasForeignKey(ut => ut.UserId);

            builder.HasMany(u => u.CompanyMemberships)
                   .WithOne(cu => cu.User)
                   .HasForeignKey(cu => cu.UserId);

            builder.HasMany(u => u.OwnedProjects)
                   .WithOne(p => p.Customer)
                   .HasForeignKey(p => p.CustomerId);

            builder.HasMany(u => u.AssignedByMe)
                   .WithOne(t => t.AssignedBy)
                   .HasForeignKey(t => t.AssignedById)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.AssignedToMe)
                   .WithOne(t => t.AssignedTo)
                   .HasForeignKey(t => t.AssignedToId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.RefreshToken)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId);
        }
    }
}
