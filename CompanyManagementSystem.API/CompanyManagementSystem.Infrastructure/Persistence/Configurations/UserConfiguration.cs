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
            // Primary Key
            builder.HasKey(u => u.UserId);

            // Properties
            builder.Property(u => u.FirstName)
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .HasMaxLength(100);

            builder.Property(u => u.UserName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasIndex(u => u.UserName)
                   .IsUnique();

            builder.Property(u => u.Password)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasMaxLength(50)
                   .HasConversion<string>();

            builder.Property(u => u.IsEmailVerfied)
                   .IsRequired();

            builder.Property(u => u.IsBanned)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            // User -> Teams he leads
            builder.HasMany(u => u.LeadingTeams)
                   .WithOne(t => t.Leader)
                   .HasForeignKey(t => t.LeaderId);

            // User <-> UserTeam
            builder.HasMany(u => u.UserTeams)
                   .WithOne(ut => ut.User)
                   .HasForeignKey(ut => ut.UserId);

            builder.HasMany(u => u.CompanyMemberships)
                   .WithOne(cu => cu.User)
                   .HasForeignKey(cu => cu.UserId);

            // User -> Owned Projects
            builder.HasMany(u => u.OwnedProjects)
                   .WithOne(p => p.Customer)
                   .HasForeignKey(p => p.CustomerId);

            // User -> Tasks created by him
            builder.HasMany(u => u.AssignedByMe)
                   .WithOne(t => t.AssignedBy)
                   .HasForeignKey(t => t.AssignedById)
                   .OnDelete(DeleteBehavior.Restrict);

            // User -> Tasks assigned to him
            builder.HasMany(u => u.AssignedToMe)
                   .WithOne(t => t.AssignedTo)
                   .HasForeignKey(t => t.AssignedToId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Refresh Tokens
            builder.HasMany(u => u.RefreshToken)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId);

            builder.HasIndex(u => u.UserName)
                   .IsUnique();
        }
    }
}
