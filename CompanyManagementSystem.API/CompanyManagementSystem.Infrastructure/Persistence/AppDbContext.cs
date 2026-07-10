using CompanyManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CompanyManagementSystem.Infrastructure.Persistence.Configurations;


namespace CompanyManagementSystem.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet<User> is already exposed by IdentityDbContext — do not redeclare it
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Tasks> Tasks { get; set; } = null!;
        public DbSet<TaskDetails> TaskDetails { get; set; } = null!;
        public DbSet<UserTeam> UserTeams { get; set; } = null!;
        public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
        public DbSet<ProjectTeam> ProjectTeams { get; set; } = null!;
        public DbSet<CompanyOffers> CompanyOffers { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must call base first — this registers all Identity tables
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TasksConfiguration());
            modelBuilder.ApplyConfiguration(new TaskDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTeamConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyOffersConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
