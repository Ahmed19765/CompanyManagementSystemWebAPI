using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler
        : IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteDepartmentCommandHandler(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository       = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<DeleteDepartmentResponse> Handle(
            DeleteDepartmentCommand request,
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

            // ── 2. Validate the department exists ─────────────────────────────────
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department is null)
                throw new Exception("Department not found.");

            // ── 3. Verify the owner owns the company this department belongs to ───
            if (department.Company?.OwnerId != request.OwnerId)
                throw new Exception("Access denied. You are not the owner of this department's company.");

            // ── 4. Count teams before deletion for the response message ───────────
            int unlinkedCount = department.Teams.Count;

            // ── 5. Delete the department ──────────────────────────────────────────
            // EF's ClientSetNull interceptor sets Team.DepartmentId = null for all
            // teams in this department before removing the department row.
            // Teams survive — they can be reassigned to another department later.
            await _departmentRepository.DeleteDepartmentAsync(request.DepartmentId);

            var message = unlinkedCount > 0
                ? $"Department deleted. {unlinkedCount} team(s) have been unlinked and need reassignment to a new department."
                : "Department deleted successfully.";

            return new DeleteDepartmentResponse
            {
                Message          = message,
                UnlinkedTeamCount = unlinkedCount
            };
        }
    }
}
