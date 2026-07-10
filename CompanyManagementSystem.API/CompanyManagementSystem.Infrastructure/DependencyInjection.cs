using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.Communication;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using CompanyManagementSystem.Infrastructure.Repositories;
using CompanyManagementSystem.Infrastructure.Services;using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register ASP.NET Core Identity without cookie auth middleware (we use JWT)
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                // We handle email confirmation ourselves via OTP + MemoryCache
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICompanyUserRepository, CompanyUserRepository>();
            services.AddScoped<ICompanyOffersRepository, CompanyOffersRepository>();
            services.AddScoped<IProjectTeamRepository, ProjectTeamRepository>();

            // Services
            // IPasswordHasher adapts Identity's IPasswordHasher<User> — BCrypt is no longer used
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAccessJwtTokenGenerator, AccessJwtTokenGenerator>();
            services.AddScoped<IRefreshJwtTokenGenerator, RefreshJwtTokenGenerator>();

            services.AddMemoryCache();

            services.AddSingleton(typeof(IMemoryCache<>), typeof(MemoryCacheService<>));
            services.AddScoped<IOtpGenerator, OtpGenerator>();
            services.AddScoped<IOtpSender<string>, OtpSender>();
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
