using CompanyManagementSystem.Application.Features.Commands.CreateTeam;
using CompanyManagementSystem.Application.Features.Queries.GetTeamMembers;
using CompanyManagementSystem.Application.Features.Queries.GetTeamTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
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
        [HttpPost("createTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        /// <summary>Owner or TeamLeader: get all members of a team.</summary>
        [Authorize(Roles = "Owner,Engineer")]
        [HttpGet("{teamId}/members")]
        public async Task<IActionResult> GetTeamMembers(int teamId)
        {
            try
            {
                var query = new GetTeamMembersQuery
                {
                    RequestingUserId = GetCurrentUserId(),
                    TeamId           = teamId
                };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner or TeamLeader: get all tasks assigned to a team.</summary>
        [Authorize(Roles = "Owner,Engineer")]
        [HttpGet("{teamId}/tasks")]
        public async Task<IActionResult> GetTeamTasks(int teamId)
        {
            try
            {
                var query = new GetTeamTasksQuery
                {
                    RequestingUserId = GetCurrentUserId(),
                    TeamId           = teamId
                };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
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
