using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteCompany
{
    public class DeleteCompanyCommandHandler
        : IRequestHandler<DeleteCompanyCommand, Response<DeleteCompanyResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteCompanyCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository,
            ITeamRepository teamRepository,
            ITaskRepository taskRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository        = userRepository;
            _companyRepository     = companyRepository;
            _companyUserRepository = companyUserRepository;
            _teamRepository        = teamRepository;
            _taskRepository        = taskRepository;
            _departmentRepository  = departmentRepository;
        }

        public async Task<Response<DeleteCompanyResponse>> Handle(
            DeleteCompanyCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the owner ──────────────────────────────────────────────
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("Owner not found.");

            if (owner.IsBanned)
                throw new Exception("Your account is banned.");

            // ── 2. Validate the company exists and belongs to this owner ──────────
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            if (company.OwnerId != request.OwnerId)
                throw new Exception("Access denied. You are not the owner of this company.");

            if (company.IsDeleted)
                throw new Exception("This company is already deleted.");

            // ── 3. Block if there are active offers ───────────────────────────────
            var hasActiveOffers = await _companyRepository.HasActiveOffersAsync(request.CompanyId);
            if (hasActiveOffers)
                throw new Exception(
                    "This company has active offers (Pending or Accepted). " +
                    "Please cancel or resolve all active offers before deleting the company.");

            // ── 4. Count members before removal ───────────────────────────────────
            var members = await _companyUserRepository.GetAllMembersByCompanyIdAsync(request.CompanyId);
            int memberCount = members.Count();

            // ── 5. Count active tasks BEFORE nullifying so the number is accurate ─
            int unassignedCount = await _taskRepository.CountActiveTasksForCompanyAsync(request.CompanyId);

            // ── 6. Nullify active task assignments across all company teams ────────
            // Active tasks (Todo, InProgress, Pending) → AssignedToId = null
            // Done / Failed tasks keep the user ID — permanent historical record.
            await _taskRepository.NullifyAllActiveTasksInCompanyAsync(request.CompanyId);

            // ── 7. Remove all members from the company ────────────────────────────
            await _companyUserRepository.RemoveAllMembersAsync(request.CompanyId);

            // ── 8. Delete all departments (teams and UserTeams cascade from EF) ───
            await _departmentRepository.DeleteAllByCompanyIdAsync(request.CompanyId);

            // ── 9. Soft-delete the company row ────────────────────────────────────
            // IsDeleted = true, DeletedAt = now.
            // Row stays in DB — CompanyOffers on old projects keep a valid CompanyId.
            await _companyRepository.SoftDeleteAsync(request.CompanyId);

            var message = unassignedCount > 0
                ? $"Company deleted. {memberCount} member(s) removed. " +
                  $"{unassignedCount} active task(s) unassigned and need reassignment."
                : $"Company deleted. {memberCount} member(s) removed.";

            var response = new DeleteCompanyResponse
            {
                Message             = message,
                RemovedMemberCount  = memberCount,
                UnassignedTaskCount = unassignedCount
            };

            return Response<DeleteCompanyResponse>.Ok(response, "Company deleted successfully.");
        }
    }
}
