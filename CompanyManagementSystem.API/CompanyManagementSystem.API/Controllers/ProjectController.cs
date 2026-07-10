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
    [Route("api/[controller]")]
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
        [HttpPost("customer/createProject")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            try
            {
                command.CustomerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: assign one of their company's projects to a team in the same company.</summary>
        [Authorize(Roles = "Owner")]
        [HttpPost("owner/assign-to-team")]
        public async Task<IActionResult> AssignProjectToTeam([FromBody] AssignProjectToTeamCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: remove a project assignment from a team (undo a wrong assignment).</summary>
        [Authorize(Roles = "Owner")]
        [HttpDelete("owner/unassign-from-team")]
        public async Task<IActionResult> UnassignProjectFromTeam([FromBody] UnassignProjectFromTeamCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>
        /// Customer: update their project info.
        /// Allowed only within 3 days of creation and only if no offer has been accepted.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpPut("customer/update-project")]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectCommand command)
        {
            try
            {
                command.CustomerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>
        /// Customer: delete their project.
        /// Allowed only within 3 days of creation and only if no offer has been accepted.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpDelete("customer/delete-project")]
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProjectCommand command)
        {
            try
            {
                command.CustomerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        /// <summary>Customer: get all their projects with task progress and offer details.</summary>
        [Authorize(Roles = "Customer")]
        [HttpGet("customer/my-projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            try
            {
                var query = new GetCustomerProjectsQuery { CustomerId = GetCurrentUserId() };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Customer: see all offers submitted by companies on one of their projects.</summary>
        [Authorize(Roles = "Customer")]
        [HttpGet("customer/project-offers")]
        public async Task<IActionResult> GetProjectOffers([FromBody] GetProjectOffersQuery query)
        {
            try
            {
                query.CustomerId = GetCurrentUserId();
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Customer: accept one company's offer — all other offers on the project are rejected.</summary>
        [Authorize(Roles = "Customer")]
        [HttpPost("customer/accept-offer")]
        public async Task<IActionResult> AcceptOffer([FromBody] AcceptProjectOfferCommand command)
        {
            try
            {
                command.CustomerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("owner/customers-projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var query = new GetProjectsQuery();
                query.OwnerId = GetCurrentUserId();
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
