using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.MemoryCache;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache<string> _memoryCache;

        public DeleteAccountCommandHandler(
            IUserRepository userRepository,
            IMemoryCache<string> memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Response<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
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

            var isOtpValid = _memoryCache.Validate(
                user.Email!,
                request.Otp,
                TypeOfValue.DeleteAccountOtp);

            if (!isOtpValid)
            {
                throw new Exception("Invalid or expired OTP.");
            }

            await _userRepository.DeleteAsync(user);

            _memoryCache.Remove(user.Email!, TypeOfValue.DeleteAccountOtp);

            return Response<string>.Ok(null!, "Account deleted successfully.");
        }
    }
}
