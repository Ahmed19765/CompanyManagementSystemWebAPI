using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace CompanyManagementSystem.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<RefreshTokenResponse>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessJwtTokenGenerator _accessTokenGenerator;
        private readonly IRefreshJwtTokenGenerator _refreshTokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IAccessJwtTokenGenerator accessTokenGenerator,
            IRefreshJwtTokenGenerator refreshTokenGenerator,
            IOptions<JwtSettings> jwtSettings)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Response<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var oldRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if (oldRefreshToken is null || !oldRefreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            var user = oldRefreshToken.User;
            if (user.IsBanned)
            {
                throw new UnauthorizedAccessException("This account is banned!");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Please verfiey your email!");
            }

            var accessToken = _accessTokenGenerator.GenerateAccessJwtToken(user);
            var newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _refreshTokenRepository.DeleteAllUserRefreshTokens(user.Id);
            await _refreshTokenRepository.CreateRefreshToken(user.Id, newRefreshToken);

            return Response<RefreshTokenResponse>.Ok(
                new RefreshTokenResponse
                {
                    AccessToken = accessToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    RefreshToken = newRefreshToken
                },
                "Token refreshed successfully.");
        }
    }
}
