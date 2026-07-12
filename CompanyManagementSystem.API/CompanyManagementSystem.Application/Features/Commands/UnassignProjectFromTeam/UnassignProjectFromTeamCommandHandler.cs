using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.UnassignProjectFromTeam
{
    public class UnassignProjectFromTeamCommandHandler
        : IRequestHandler<UnassignProjectFromTeamCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectTeamRepository _projectTeamRepository;

        public UnassignProjectFromTeamCommandHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            IProjectTeamRepository projectTeamRepository)
        {
            _userRepository        = userRepository;
            _teamRepository        = teamRepository;
            _projectTeamRepository = projectTeamRepository;
        }

        public async Task<Response<string>> Handle(
            UnassignProjectFromTeamCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the owner ──────────────────────────────────────────────
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("User not found.");

            if (owner.IsBanned)
                throw new Exception("Your account is banned.");

            if (!owner.EmailConfirmed)
                throw new Exception("Please verify your email.");

            if (owner.Role != UserRole.Owner)
                throw new Exception("Only owners can unassign projects from teams.");

            // ── 2. Validate team belongs to this owner's company ──────────────────
            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team is null)
                throw new Exception("Team not found.");

            if (team.Department is null)
                throw new Exception("This team is not linked to any department.");

            if (team.Department.Company?.OwnerId != request.OwnerId)
                throw new Exception("Access denied. This team does not belong to your company.");

            // ── 3. Validate the assignment actually exists ─────────────────────────
            var isAssigned = await _projectTeamRepository.IsAlreadyAssignedAsync(
                request.ProjectId,
                request.TeamId);

            if (!isAssigned)
                throw new Exception("This team is not assigned to this project.");

            // ── 4. Remove the assignment ───────────────────────────────────────────
            await _projectTeamRepository.UnassignAsync(request.ProjectId, request.TeamId);

            return Response<string>.Ok(null!, "Project unassigned from team successfully.");
        }
    }
}
