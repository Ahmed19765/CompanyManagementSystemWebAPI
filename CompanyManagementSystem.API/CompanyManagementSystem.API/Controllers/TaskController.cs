using CompanyManagementSystem.Application.Features.Commands.Tasks;
using CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus;
using CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Commands ───────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner,Engineer")]
        [HttpPost("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] AddTaskCommand command)
        {
            try
            {
                command.CurrentUserId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>
        /// Engineer: update the status of a task assigned to them.
        /// Member rank: can set Todo, InProgress, Pending.
        /// Leader rank: can set any status including Done and Failed.
        /// </summary>
        [Authorize(Roles = "Engineer")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusCommand command)
        {
            try
            {
                command.UserId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }
        // ── Queries ────────────────────────────────────────────────────────────────

        /// <summary>Engineer (member): get all tasks assigned to me.</summary>
        [Authorize(Roles = "Engineer")]
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            try
            {
                var query = new GetMyAssignedTasksQuery { UserId = GetCurrentUserId() };
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
