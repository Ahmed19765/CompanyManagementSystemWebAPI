using CompanyManagementSystem.Application.Features.Commands.AddTeamMember;
using CompanyManagementSystem.Application.Features.Commands.CreateTeam;
using CompanyManagementSystem.Application.Features.Commands.DeleteTeam;
using CompanyManagementSystem.Application.Features.Commands.RemoveTeamMember;
using CompanyManagementSystem.Application.Features.Queries.GetTeamMembers;
using CompanyManagementSystem.Application.Features.Queries.GetTeamTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Commands ───────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner")]
        [HttpPost("api/owner/teams")]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("api/owner/teams")]
        public async Task<IActionResult> DeleteTeam([FromBody] DeleteTeamCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("api/owner/teams/members")]
        public async Task<IActionResult> AddTeamMember([FromBody] AddTeamMemberCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("api/owner/teams/members")]
        public async Task<IActionResult> RemoveTeamMember([FromBody] RemoveTeamMemberCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner,Engineer")]
        [HttpGet("api/teams/{teamId}/members")]
        public async Task<IActionResult> GetTeamMembers(Guid teamId)
        {
            var query = new GetTeamMembersQuery
            {
                RequestingUserId = GetCurrentUserId(),
                TeamId           = teamId
            };
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Owner,Engineer")]
        [HttpGet("api/teams/{teamId}/tasks")]
        public async Task<IActionResult> GetTeamTasks(Guid teamId)
        {
            var query = new GetTeamTasksQuery
            {
                RequestingUserId = GetCurrentUserId(),
                TeamId           = teamId
            };
            return Ok(await _mediator.Send(query));
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

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
