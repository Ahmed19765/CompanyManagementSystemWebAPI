using CompanyManagementSystem.Application.Features.Commands.Login;
using CompanyManagementSystem.Application.Features.Commands.Register;
using CompanyManagementSystem.Application.Features.Commands.ResendEmailVerificationOtp;
using CompanyManagementSystem.Application.Features.Commands.RequestPasswordReset;
using CompanyManagementSystem.Application.Features.Commands.ResetPassword;
using CompanyManagementSystem.Application.Features.Commands.RefreshToken;
using CompanyManagementSystem.Application.Features.Commands.VerifyEmail;
using CompanyManagementSystem.Application.Features.Commands.DeleteUserAccount;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("resend-email-verification-otp")]
        public async Task<IActionResult> ResendEmailVerificationOtp([FromBody] ResendEmailVerificationOtpCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("request-delete-account")]
        public async Task<IActionResult> RequestDeleteAccount()
        {
            var command = new RequestDeleteAccountCommand
            {
                UserId = GetCurrentUserId()
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand command)
        {
            command.UserId = GetCurrentUserId();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        private Guid GetCurrentUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("sub");

            if (!Guid.TryParse(value, out var id))
                throw new Exception("Invalid token.");

            return id;
        }
    }
}
