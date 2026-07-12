using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyManagementSystem.Application.Features.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IOtpSender<string> _otpSender;
        private readonly IMemoryCache<string> _memoryCache;
        private readonly ILogger<RequestPasswordResetCommandHandler> _logger;

        public RequestPasswordResetCommandHandler(
            IUserRepository userRepository,
            IOtpGenerator otpGenerator,
            IOtpSender<string> otpSender,
            IMemoryCache<string> memoryCache,
            ILogger<RequestPasswordResetCommandHandler> logger)
        {
            _userRepository = userRepository;
            _otpGenerator = otpGenerator;
            _otpSender = otpSender;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Response<string>.Ok(null!, "If the email exists, a reset OTP has been sent.");
            }

            if (user.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Please verify your email before resetting your password!");
            }

            var otp = _otpGenerator.GenerateOtp();

            _memoryCache.Save(
                request.Email,
                otp,
                TypeOfValue.PasswordResetOtp,
                TimeSpan.FromMinutes(5));

            await _otpSender.SendOtpAsync(
                request.Email,
                otp,
                SenderType.Email);

            _logger.LogInformation(
                "Password reset OTP sent successfully to {Email}",
                request.Email);

            return Response<string>.Ok(null!, "If the email exists, a reset OTP has been sent.");
        }
    }
}
