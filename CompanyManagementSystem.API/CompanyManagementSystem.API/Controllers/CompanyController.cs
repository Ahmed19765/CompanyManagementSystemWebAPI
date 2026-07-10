using CompanyManagementSystem.Application.Features.Commands.AddCompanyMember;
using CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer;
using CompanyManagementSystem.Application.Features.Commands.CreateCompany;
using CompanyManagementSystem.Application.Features.Commands.DeleteCompany;
using CompanyManagementSystem.Application.Features.Commands.RemoveCompanyMember;
using CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank;
using CompanyManagementSystem.Application.Features.Queries.GetCompanyAcceptedProjects;
using CompanyManagementSystem.Application.Features.Queries.GetCompanyDepartments;
using CompanyManagementSystem.Application.Features.Queries.GetCompanyMembers;
using CompanyManagementSystem.Application.Features.Queries.GetOwnerCompanies;
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

        // ── Commands ───────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner")]
        [HttpPost("owner/create-company")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("owner/company-members/rank-up")]
        public async Task<IActionResult> SetCompanyUserRank([FromBody] SetCompanyUserRankCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("owner/add-members")]
        public async Task<IActionResult> AddCompanyMember([FromBody] AddCompanyMemberCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: submit an offer from one of their companies on a pending project.</summary>
        [Authorize(Roles = "Owner")]
        [HttpPost("owner/add-offer")]
        public async Task<IActionResult> AddCompanyOffer([FromBody] AddCompanyOfferCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: remove a specific member from their company.</summary>
        [Authorize(Roles = "Owner")]
        [HttpDelete("owner/remove-member")]
        public async Task<IActionResult> RemoveCompanyMember([FromBody] RemoveCompanyMemberCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: soft-delete a company — dissolves all teams/members, preserves project history.</summary>
        [Authorize(Roles = "Owner")]
        [HttpDelete("owner/delete-company")]
        public async Task<IActionResult> DeleteCompany([FromBody] DeleteCompanyCommand command)
        {
            try
            {
                command.OwnerId = GetCurrentUserId();
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        /// <summary>Owner: get all companies they own.</summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("owner/my-companies")]
        public async Task<IActionResult> GetMyCompanies()
        {
            try
            {
                var query = new GetOwnerCompaniesQuery { OwnerId = GetCurrentUserId() };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: get all members of a specific company they own.</summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("owner/{companyId}/members")]
        public async Task<IActionResult> GetCompanyMembers(int companyId)
        {
            try
            {
                var query = new GetCompanyMembersQuery
                {
                    RequestingUserId = GetCurrentUserId(),
                    CompanyId        = companyId
                };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>Owner: get all departments of a specific company they own.</summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("owner/{companyId}/departments")]
        public async Task<IActionResult> GetCompanyDepartments(int companyId)
        {
            try
            {
                var query = new GetCompanyDepartmentsQuery
                {
                    RequestingUserId = GetCurrentUserId(),
                    CompanyId        = companyId
                };
                return Ok(await _mediator.Send(query));
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        /// <summary>
        /// Owner: get all projects their company has an accepted offer on,
        /// with progress = Done tasks / Total tasks. Returns 0% if no tasks yet.
        /// </summary>
        [Authorize(Roles = "Owner")]
        [HttpGet("owner/accepted-projects")]
        public async Task<IActionResult> GetAcceptedProjects([FromBody] GetCompanyAcceptedProjectsQuery query)
        {
            try
            {
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
