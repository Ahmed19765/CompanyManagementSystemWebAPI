using CompanyManagementSystem.Application.Common;
using MediatR;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Application.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessJwtTokenGenerator _accessTokenGenerator;
        private readonly IRefreshJwtTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAccessJwtTokenGenerator accessTokenGenerator,
            IRefreshJwtTokenGenerator refreshTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Response<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Get user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash!))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (user.IsBanned)
            {
                throw new UnauthorizedAccessException("This account is banned!");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Please verfiey your email!");
            }

            // 3. Generate tokens
            var accessToken = _accessTokenGenerator.GenerateAccessJwtToken(user);
            var refreshTokenString = _refreshTokenGenerator.GenerateRefreshToken();

            // 4. Replace all old refresh tokens with one new token
            await _refreshTokenRepository.DeleteAllUserRefreshTokens(user.Id);
            await _refreshTokenRepository.CreateRefreshToken(user.Id, refreshTokenString);

            return Response<LoginResponse>.Ok(
                new LoginResponse
                {
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Username = user.UserName ?? string.Empty,
                    AccessToken = accessToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    RefreshToken = refreshTokenString
                },
                "Login successful.");
        }
    }
}
