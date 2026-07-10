using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveCompanyMember
{
    public class RemoveCompanyMemberCommandHandler
        : IRequestHandler<RemoveCompanyMemberCommand, RemoveCompanyMemberResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITaskRepository _taskRepository;

        public RemoveCompanyMemberCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository,
            ITeamRepository teamRepository,
            ITaskRepository taskRepository)
        {
            _userRepository       = userRepository;
            _companyRepository    = companyRepository;
            _companyUserRepository = companyUserRepository;
            _teamRepository       = teamRepository;
            _taskRepository       = taskRepository;
        }

        public async Task<RemoveCompanyMemberResponse> Handle(
            RemoveCompanyMemberCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the owner ──────────────────────────────────────────────
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("Owner not found.");

            if (owner.IsBanned)
                throw new Exception("Your account is banned.");

            // ── 2. Validate the company belongs to this owner ─────────────────────
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            if (company.OwnerId != request.OwnerId)
                throw new Exception("Access denied. You are not the owner of this company.");

            // ── 3. Validate the target user is actually a member ──────────────────
            var isMember = await _companyUserRepository.IsMemberAsync(
                request.CompanyId,
                request.TargetUserId);

            if (!isMember)
                throw new Exception("This user is not a member of your company.");

            // Owner cannot remove themselves — they are the company, not a member
            if (request.TargetUserId == request.OwnerId)
                throw new Exception("You cannot remove yourself as the company owner.");

            // ── 4. Count active tasks before nullifying (for the response message) ─
            var activeTasks = await _taskRepository.GetAllAssignedToUserAsync(request.TargetUserId);
            var activeStates = new[] { TaskState.Todo, TaskState.InProgress, TaskState.Pending };
            var unassignedCount = activeTasks.Count(t => activeStates.Contains(t.Status));

            // ── 5. Nullify active task assignments ────────────────────────────────
            // Done / Failed tasks keep the user's Id — they are a historical record.
            await _taskRepository.NullifyPendingTaskAssignmentsAsync(request.TargetUserId);

            // ── 6. Remove user from all teams in this company ─────────────────────
            // This deletes UserTeam rows and nulls Team.LeaderId where applicable.
            await _teamRepository.RemoveUserFromAllTeamsInCompanyAsync(
                request.CompanyId,
                request.TargetUserId);

            // ── 7. Remove the company membership ─────────────────────────────────
            await _companyUserRepository.RemoveMemberAsync(
                request.CompanyId,
                request.TargetUserId);

            var message = unassignedCount > 0
                ? $"Member removed successfully. {unassignedCount} active task(s) have been unassigned and need reassignment."
                : "Member removed successfully.";

            return new RemoveCompanyMemberResponse
            {
                Message             = message,
                UnassignedTaskCount = unassignedCount
            };
        }
    }
}
