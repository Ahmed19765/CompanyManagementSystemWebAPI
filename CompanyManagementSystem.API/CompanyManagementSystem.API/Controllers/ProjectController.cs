using CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer;
using CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam;
using CompanyManagementSystem.Application.Features.Commands.CreateProject;
using CompanyManagementSystem.Application.Features.Commands.DeleteProject;
using CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam;
using CompanyManagementSystem.Application.Features.Commands.UpdateProject;
using CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects;
using CompanyManagementSystem.Application.Features.Queries.GetProjectOffers;
using CompanyManagementSystem.Application.Features.Queries.GetProjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyManagementSystem.API.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ── Commands ───────────────────────────────────────────────────────────────

        [Authorize(Roles = "Customer")]
        [HttpPost("api/customer/projects")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            command.CustomerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("api/owner/projects/assign-to-team")]
        public async Task<IActionResult> AssignProjectToTeam([FromBody] AssignProjectToTeamCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("api/owner/projects/unassign-from-team")]
        public async Task<IActionResult> UnassignProjectFromTeam([FromBody] UnassignProjectFromTeamCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("api/customer/projects")]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectCommand command)
        {
            command.CustomerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("api/customer/projects")]
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProjectCommand command)
        {
            command.CustomerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        [Authorize(Roles = "Customer")]
        [HttpGet("api/customer/projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var query = new GetCustomerProjectsQuery { CustomerId = GetCurrentUserId() };
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("api/customer/projects/offers")]
        public async Task<IActionResult> GetProjectOffers([FromBody] GetProjectOffersQuery query)
        {
            query.CustomerId = GetCurrentUserId();
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("api/customer/projects/accept-offer")]
        public async Task<IActionResult> AcceptOffer([FromBody] AcceptProjectOfferCommand command)
        {
            command.CustomerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("api/owner/projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var query = new GetProjectsQuery();
            query.OwnerId = GetCurrentUserId();
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
