using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.Communication;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Infrastructure.Persistence;
using CompanyManagementSystem.Infrastructure.Repositories;
using CompanyManagementSystem.Infrastructure.Services;
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

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICompanyUserRepository, CompanyUserRepository>();

            // Services
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
