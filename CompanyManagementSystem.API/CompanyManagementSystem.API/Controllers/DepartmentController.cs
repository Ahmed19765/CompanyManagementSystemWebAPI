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
    [Route("api/[controller]")]
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
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] UpdateDepartmentCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                command.DepartmentId = departmentId;
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: delete a department — teams are unlinked (DepartmentId = null) not deleted.</summary>
        [Authorize(Roles = "Owner")]
        [HttpDelete("owner/delete-department")]
        public async Task<IActionResult> DeleteDepartment([FromBody] DeleteDepartmentCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        /// <summary>Owner: get all teams inside a department they own.</summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("{departmentId}/teams")]
        public async Task<IActionResult> GetDepartmentTeams(int departmentId)
        {
            try
            {
                var query = new GetDepartmentTeamsQuery
                {
                    RequestingUserId = GetCurrentUserId(),
                    DepartmentId     = departmentId
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
