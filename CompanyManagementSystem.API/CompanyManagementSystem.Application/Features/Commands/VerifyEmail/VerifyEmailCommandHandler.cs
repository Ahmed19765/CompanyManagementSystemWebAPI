using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache<string> _memoryCache;

        public VerifyEmailCommandHandler(
            IUserRepository userRepository,
            IMemoryCache<string> memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<VerifyEmailResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (user.EmailConfirmed)
            {
                return new VerifyEmailResponse
                {
                    Message = "Email is already verified."
                };
            }

            var isOtpValid = _memoryCache.Validate(
                request.Email,
                request.Otp,
                TypeOfValue.EmailVerificationOtp);

            if (!isOtpValid)
            {
                throw new Exception("Invalid or expired OTP.");
            }

            user.EmailConfirmed = true;
            // UpdateAsync now goes through UserManager.UpdateAsync — it saves internally.
            await _userRepository.UpdateAsync(user);
            // await _userRepository.SaveChangesAsync(); // not needed — UserManager saves on its own

            _memoryCache.Remove(request.Email, TypeOfValue.EmailVerificationOtp);

            return new VerifyEmailResponse
            {
                Message = "Email verified successfully."
            };
        }
    }
}
