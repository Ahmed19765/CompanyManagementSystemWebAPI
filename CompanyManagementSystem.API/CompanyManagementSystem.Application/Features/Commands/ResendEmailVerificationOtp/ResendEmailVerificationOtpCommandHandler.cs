using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyManagementSystem.Application.Features.Commands.ResendEmailVerificationOtp
{
    public class ResendEmailVerificationOtpCommandHandler : IRequestHandler<ResendEmailVerificationOtpCommand, ResendEmailVerificationOtpResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IOtpSender<string> _otpSender;
        private readonly IMemoryCache<string> _memoryCache;
        private readonly ILogger<ResendEmailVerificationOtpCommandHandler> _logger;

        public ResendEmailVerificationOtpCommandHandler(
            IUserRepository userRepository,
            IOtpGenerator otpGenerator,
            IOtpSender<string> otpSender,
            IMemoryCache<string> memoryCache,
            ILogger<ResendEmailVerificationOtpCommandHandler> logger)
        {
            _userRepository = userRepository;
            _otpGenerator = otpGenerator;
            _otpSender = otpSender;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ResendEmailVerificationOtpResponse> Handle(ResendEmailVerificationOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (user.IsEmailVerfied)
            {
                return new ResendEmailVerificationOtpResponse
                {
                    Message = "Email is already verified."
                };
            }

            if (user.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            var otp = _otpGenerator.GenerateOtp();

            _memoryCache.Save(
                request.Email,
                otp,
                TypeOfValue.EmailVerificationOtp,
                TimeSpan.FromMinutes(5));

            await _otpSender.SendOtpAsync(
                request.Email,
                otp,
                SenderType.Email);

            _logger.LogInformation(
                "Email verification OTP resent successfully to {Email}",
                request.Email);

            return new ResendEmailVerificationOtpResponse
            {
                Message = "Email verification OTP sent successfully."
            };
        }
    }
}
