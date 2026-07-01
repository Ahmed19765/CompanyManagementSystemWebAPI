using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace CompanyManagementSystem.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
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

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var oldRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if (oldRefreshToken is null || !oldRefreshToken.IsActive)
            {
                throw new Exception("Invalid refresh token.");
            }

            var user = oldRefreshToken.User;
            if (user.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            if (!user.IsEmailVerfied)
            {
                throw new Exception("Please verfiey your email!");
            }

            var accessToken = _accessTokenGenerator.GenerateAccessJwtToken(user);
            var newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _refreshTokenRepository.DeleteAllUserRefreshTokens(user.UserId);
            await _refreshTokenRepository.CreateRefreshToken(user.UserId, newRefreshToken);

            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                RefreshToken = newRefreshToken
            };
        }
    }
}
