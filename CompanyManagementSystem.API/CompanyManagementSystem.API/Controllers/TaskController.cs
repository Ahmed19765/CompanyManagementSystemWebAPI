using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CompanyManagementSystem.Application.Features.Commands.Tasks;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CompanyManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public TaskController(IMediator mediator) 
        {
            _mediatR = mediator;
        }

        [HttpPost("createTask")]
        public async Task<IActionResult> CreateTask([FromBody] AddTaskCommand Command)
        {
            // Extract the user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User is not authorized." });
            }

            Command.CurrentUserId = userId;

            try
            {
                var result = await _mediatR.Send(Command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
