using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CompanyManagementSystem.Seeder.Models;
using CompanyManagementSystem.Seeder.Models.Enums;

namespace CompanyManagementSystem.Seeder.Data
{
    /// <summary>
    /// Lightweight DbContext that maps exactly to the same tables
    /// the main API's AppDbContext creates via EF migrations.
    /// No navigation properties — we only need to INSERT rows.
    /// </summary>
    public class SeederDbContext : IdentityDbContext<SeederUser, IdentityRole<Guid>, Guid>
    {
        public SeederDbContext(DbContextOptions<SeederDbContext> options) : base(options) { }

        public DbSet<SeederCompany>      Companies     { get; set; } = null!;
        public DbSet<SeederDepartment>   Departments   { get; set; } = null!;
        public DbSet<SeederTeam>         Teams         { get; set; } = null!;
        public DbSet<SeederUserTeam>     UserTeams     { get; set; } = null!;
        public DbSet<SeederCompanyUser>  CompanyUsers  { get; set; } = null!;
        public DbSet<SeederProject>      Projects      { get; set; } = null!;
        public DbSet<SeederProjectTeam>  ProjectTeams  { get; set; } = null!;
        public DbSet<SeederCompanyOffer> CompanyOffers { get; set; } = null!;
        public DbSet<SeederTask>         Tasks         { get; set; } = null!;
        public DbSet<SeederTaskDetails>  TaskDetails   { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MUST call base — registers all Identity tables (AspNetUsers, AspNetRoles, …)
            base.OnModelCreating(modelBuilder);

            // ── SeederUser extra columns ───────────────────────────────────
            modelBuilder.Entity<SeederUser>(b =>
            {
                b.Property(u => u.FirstName).HasMaxLength(100);
                b.Property(u => u.LastName).HasMaxLength(100);
                b.Property(u => u.Role).HasMaxLength(50).HasConversion<string>();
                b.Property(u => u.IsBanned).IsRequired();
                b.Property(u => u.CreatedAt).IsRequired();
            });

            // ── Company ────────────────────────────────────────────────────
            modelBuilder.Entity<SeederCompany>(b =>
            {
                b.ToTable("Companies");
                b.HasKey(c => c.CompanyId);
                b.Property(c => c.CompanyName).HasMaxLength(200);
                b.Property(c => c.JoinCode).HasMaxLength(100);
            });

            // ── CompanyUser (composite PK) ─────────────────────────────────
            modelBuilder.Entity<SeederCompanyUser>(b =>
            {
                b.ToTable("CompanyUsers");
                b.HasKey(cu => new { cu.CompanyId, cu.UserId });
                b.Property(cu => cu.Rank).HasMaxLength(50).HasConversion<string>().IsRequired();
                b.Property(cu => cu.JoinedAt).IsRequired();
            });

            // ── Department ─────────────────────────────────────────────────
            modelBuilder.Entity<SeederDepartment>(b =>
            {
                b.ToTable("Departments");
                b.HasKey(d => d.DepartmentId);
                b.Property(d => d.DepartmentName).HasMaxLength(200);
            });

            // ── Team ───────────────────────────────────────────────────────
            modelBuilder.Entity<SeederTeam>(b =>
            {
                b.ToTable("Teams");
                b.HasKey(t => t.TeamId);
                b.Property(t => t.TeamName).HasMaxLength(200);
                b.Property(t => t.TeamDescription).HasMaxLength(1000);
            });

            // ── UserTeam (composite PK) ────────────────────────────────────
            modelBuilder.Entity<SeederUserTeam>(b =>
            {
                b.ToTable("UserTeams");
                b.HasKey(ut => new { ut.UserId, ut.TeamId });
                b.Property(ut => ut.TeamRole).HasMaxLength(100);
            });

            // ── Project ────────────────────────────────────────────────────
            modelBuilder.Entity<SeederProject>(b =>
            {
                b.ToTable("Projects");
                b.HasKey(p => p.ProjectId);
                b.Property(p => p.ProjectTitle).HasMaxLength(300);
                b.Property(p => p.ProjectOfferedBudget).HasColumnType("decimal(18,2)");
            });

            // ── ProjectTeam (composite PK) ─────────────────────────────────
            modelBuilder.Entity<SeederProjectTeam>(b =>
            {
                b.ToTable("ProjectTeams");
                b.HasKey(pt => new { pt.ProjectId, pt.TeamId });
            });

            // ── CompanyOffer (composite PK) ────────────────────────────────
            modelBuilder.Entity<SeederCompanyOffer>(b =>
            {
                b.ToTable("CompanyOffers");
                b.HasKey(co => new { co.CompanyId, co.ProjectId });
                b.Property(co => co.OfferedBudget).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(co => co.StartDate).IsRequired();
                b.Property(co => co.DeliveryExceptedDate).IsRequired();
                b.Property(co => co.Status).HasMaxLength(20).HasConversion<string>();
            });

            // ── Task ───────────────────────────────────────────────────────
            modelBuilder.Entity<SeederTask>(b =>
            {
                b.ToTable("Tasks");
                b.HasKey(t => t.TaskId);
                b.Property(t => t.TaskTitle).HasMaxLength(300);
                b.Property(t => t.Status).HasMaxLength(50).HasConversion<string>();
            });

            // ── TaskDetails ────────────────────────────────────────────────
            modelBuilder.Entity<SeederTaskDetails>(b =>
            {
                b.ToTable("TaskDetails");
                b.HasKey(td => td.TaskDetailsId);
                b.HasIndex(td => td.TaskId).IsUnique(); // 1-to-1
            });
        }
    }
}
