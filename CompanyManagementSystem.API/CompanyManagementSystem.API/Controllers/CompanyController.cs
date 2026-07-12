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
    [Route("api/owner")]
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
        [HttpPost("companies")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("companies/members/rank-up")]
        public async Task<IActionResult> SetCompanyUserRank([FromBody] SetCompanyUserRankCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("companies/members")]
        public async Task<IActionResult> AddCompanyMember([FromBody] AddCompanyMemberCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("companies/offers")]
        public async Task<IActionResult> AddCompanyOffer([FromBody] AddCompanyOfferCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("companies/members")]
        public async Task<IActionResult> RemoveCompanyMember([FromBody] RemoveCompanyMemberCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("companies")]
        public async Task<IActionResult> DeleteCompany([FromBody] DeleteCompanyCommand command)
        {
            command.OwnerId = GetCurrentUserId();
            return Ok(await _mediator.Send(command));
        }

        // ── Queries ────────────────────────────────────────────────────────────────

        [Authorize(Roles = "Owner")]
        [HttpGet("companies")]
        public async Task<IActionResult> GetMyCompanies()
        {
            var query = new GetOwnerCompaniesQuery { OwnerId = GetCurrentUserId() };
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("companies/{companyId}/members")]
        public async Task<IActionResult> GetCompanyMembers(Guid companyId)
        {
            var query = new GetCompanyMembersQuery
            {
                RequestingUserId = GetCurrentUserId(),
                CompanyId        = companyId
            };
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("companies/{companyId}/departments")]
        public async Task<IActionResult> GetCompanyDepartments(Guid companyId)
        {
            var query = new GetCompanyDepartmentsQuery
            {
                RequestingUserId = GetCurrentUserId(),
                CompanyId        = companyId
            };
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("companies/accepted-projects")]
        public async Task<IActionResult> GetAcceptedProjects([FromBody] GetCompanyAcceptedProjectsQuery query)
        {
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
