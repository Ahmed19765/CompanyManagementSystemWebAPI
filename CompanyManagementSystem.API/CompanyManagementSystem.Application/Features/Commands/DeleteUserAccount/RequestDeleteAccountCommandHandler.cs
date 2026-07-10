using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class RequestDeleteAccountCommandHandler : IRequestHandler<RequestDeleteAccountCommand, RequestDeleteAccountResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IOtpSender<string> _otpSender;
        private readonly IMemoryCache<string> _memoryCache;
        private readonly ILogger<RequestDeleteAccountCommandHandler> _logger;

        public RequestDeleteAccountCommandHandler(
            IUserRepository userRepository,
            IOtpGenerator otpGenerator,
            IOtpSender<string> otpSender,
            IMemoryCache<string> memoryCache,
            ILogger<RequestDeleteAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _otpGenerator = otpGenerator;
            _otpSender = otpSender;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<RequestDeleteAccountResponse> Handle(RequestDeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (user.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            var otp = _otpGenerator.GenerateOtp();

            _memoryCache.Save(
                user.Email!,
                otp,
                TypeOfValue.DeleteAccountOtp,
                TimeSpan.FromMinutes(5));

            await _otpSender.SendOtpAsync(
                user.Email!,
                otp,
                SenderType.Email);

            _logger.LogInformation(
                "Account deletion OTP sent successfully to {Email}",
                user.Email);

            return new RequestDeleteAccountResponse
            {
                Message = "Account deletion OTP sent successfully to your registered email."
            };
        }
    }
}
