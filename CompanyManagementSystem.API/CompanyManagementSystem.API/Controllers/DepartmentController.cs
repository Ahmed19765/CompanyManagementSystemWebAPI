using CompanyManagementSystem.Application.Features.Commands.CreateDepartment;
using CompanyManagementSystem.Application.Features.Commands.DeleteDepartment;
using CompanyManagementSystem.Application.Features.Commands.UpdateDepartment;
using CompanyManagementSystem.Application.Features.Queries.GetDepartmentTeams;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [Route("api/owner/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Commands ───────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(Guid departmentId, [FromBody] UpdateDepartmentCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            command.DepartmentId = departmentId;
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner")]
        [HttpGet("{departmentId}/teams")]
        public async Task<IActionResult> GetDepartmentTeams(Guid departmentId)
        {
            var query = new GetDepartmentTeamsQuery
            {
                RequestingUserId = GetCurrentUserId(),
                DepartmentId     = departmentId
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
