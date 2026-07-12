using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        // IPasswordHasher no longer used here — UpdatePasswordAsync goes through UserManager.
        // Kept as comment so you know why it was removed.
        // private readonly IPasswordHasher _passwordHasher;
        private readonly IMemoryCache<string> _memoryCache;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            // IPasswordHasher passwordHasher,
            IMemoryCache<string> memoryCache,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            // _passwordHasher = passwordHasher;
            _memoryCache = memoryCache;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                throw new Exception("User not found.");
            }

            if (user.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            var isOtpValid = _memoryCache.Validate(
                request.Email,
                request.Otp,
                TypeOfValue.PasswordResetOtp);

            if (!isOtpValid)
            {
                throw new Exception("Invalid or expired OTP.");
            }

            // UpdatePasswordAsync goes through UserManager.RemovePasswordAsync + AddPasswordAsync.
            // This correctly rehashes, updates SecurityStamp, and persists — all in one call.
            // OLD approach was: user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            //                   await _userRepository.UpdateAsync(user);
            //                   await _userRepository.SaveChangesAsync();
            await _userRepository.UpdatePasswordAsync(user, request.NewPassword);

            await _refreshTokenRepository.DeleteAllUserRefreshTokens(user.Id);
            _memoryCache.Remove(request.Email, TypeOfValue.PasswordResetOtp);

            return Response<string>.Ok(null!, "Password has been reset successfully.");
        }
    }
}
