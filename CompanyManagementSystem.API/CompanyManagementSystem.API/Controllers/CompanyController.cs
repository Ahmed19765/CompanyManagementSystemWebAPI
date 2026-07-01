using CompanyManagementSystem.Application.Features.Commands.CreateCompany;
using CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ISender _mediator;

        public CompanyController(ISender mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("company")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyCommand command)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!Guid.TryParse(userId, out var ownerId))
                {
                    return Unauthorized(new { Message = "Invalid token." });
                }

                command.OwnerId = ownerId;

                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("company-users/rank")]
        public async Task<IActionResult> SetCompanyUserRank([FromBody] SetCompanyUserRankCommand command)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

                if (!Guid.TryParse(userId, out var ownerId))
                {
                    return Unauthorized(new { Message = "Invalid token." });
                }

                command.OwnerId = ownerId;

                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
