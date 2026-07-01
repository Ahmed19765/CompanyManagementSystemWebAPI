using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IOtpSender<string> _otpSender;
        private readonly IMemoryCache<string> _memoryCache;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IPasswordHasher passwordHasher,
            IOtpGenerator otpGenerator,
            IOtpSender<string> otpSender,
            IMemoryCache<string> memoryCache,
            ILogger<RegisterCommandHandler> logger)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _passwordHasher = passwordHasher;
            _otpGenerator = otpGenerator;
            _otpSender = otpSender;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<RegisterResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Check if user already exists
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                throw new Exception("Email is already registered.");
            }

            if (await _userRepository.ExistsByUserNameAsync(request.UserName))
            {
                throw new Exception("Username is already taken.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                Password = _passwordHasher.HashPassword(request.Password),
                Role = request.Role,
                IsEmailVerfied = false,
                IsBanned = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // 5. Send OTP for verification
            var otp = _otpGenerator.GenerateOtp();

            _memoryCache.Save(
                user.Email!,
                otp,
                TypeOfValue.EmailVerificationOtp,
                TimeSpan.FromMinutes(2));

            try
            {
                await _otpSender.SendOtpAsync(
                    user.Email!,
                    otp,
                    SenderType.Email);

                _logger.LogInformation(
                    "OTP sent successfully to {Email}",
                    user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send OTP to {Email}",
                    user.Email);
            }

            return new RegisterResponse
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Message = $"Registration successful. Welcome {user.FirstName} {user.LastName}"
            };
        }
    }
}
