using CompanyManagementSystem.Application.Features.Commands.DeleteTask;
using CompanyManagementSystem.Application.Features.Commands.Tasks;
using CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus;
using CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
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
        [HttpPost("api/tasks")]
        public async Task<IActionResult> CreateTask([FromBody] AddTaskCommand command)
        {
            command.CurrentUserId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner,Engineer")]
        [HttpDelete("api/tasks")]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
        {
            command.CurrentUserId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Engineer")]
        [HttpPut("api/engineer/tasks/status")]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusCommand command)
        {
            command.UserId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }
        // ── Queries ────────────────────────────────────────────────────────────────

        [Authorize(Roles = "Engineer")]
        [HttpGet("api/engineer/tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var query = new GetMyAssignedTasksQuery { UserId = GetCurrentUserId() };
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
