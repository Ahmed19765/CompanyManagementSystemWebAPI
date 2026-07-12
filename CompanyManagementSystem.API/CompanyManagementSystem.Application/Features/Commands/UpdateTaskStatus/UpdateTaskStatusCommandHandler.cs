using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler
        : IRequestHandler<UpdateTaskStatusCommand, Response<UpdateTaskStatusResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        // Statuses a regular member (non-leader Engineer) is allowed to set
        private static readonly TaskState[] MemberAllowedStatuses =
        {
            TaskState.Todo,
            TaskState.InProgress,
            TaskState.Pending
        };

        public UpdateTaskStatusCommandHandler(
            IUserRepository userRepository,
            ITaskRepository taskRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository        = userRepository;
            _taskRepository        = taskRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Response<UpdateTaskStatusResponse>> Handle(
            UpdateTaskStatusCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the user ───────────────────────────────────────────────
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                throw new Exception("User not found.");

            if (user.IsBanned)
                throw new Exception("Your account is banned.");

            if (!user.EmailConfirmed)
                throw new Exception("Please verify your email.");

            // ── 2. Load the task with team/company context ─────────────────────────
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task is null)
                throw new Exception("Task not found.");

            // ── 3. Task must be assigned to this user ──────────────────────────────
            if (task.AssignedToId != request.UserId)
                throw new Exception("Access denied. This task is not assigned to you.");

            // ── 4. Resolve company and check membership ────────────────────────────
            var companyId = task.Team?.Department?.CompanyId ?? Guid.Empty;
            if (companyId == Guid.Empty)
                throw new Exception("Task's team or department is no longer linked to a company.");

            var membership = await _companyUserRepository.GetMembershipAsync(companyId, request.UserId);
            if (membership is null)
                throw new Exception("Access denied. You are not a member of this company.");

            // ── 5. Enforce status rules based on company rank ──────────────────────
            bool isLeader = membership.Rank == CompanyRank.Leader;

            if (!isLeader && !MemberAllowedStatuses.Contains(request.NewStatus))
            {
                // Member tried to set Done or Failed — only leaders can do that
                throw new Exception(
                    $"Access denied. Members can only set status to: " +
                    $"{string.Join(", ", MemberAllowedStatuses.Select(s => s.ToString()))}. " +
                    $"Only a team leader can mark a task as Done or Failed.");
            }

            // ── 6. Apply the status change ─────────────────────────────────────────
            task.Status = request.NewStatus;
            await _taskRepository.SaveChangesAsync();

            return Response<UpdateTaskStatusResponse>.Ok(new UpdateTaskStatusResponse
            {
                TaskId    = task.TaskId,
                TaskTitle = task.TaskTitle ?? string.Empty,
                NewStatus = task.Status.ToString(),
                Message   = $"Task status updated to '{task.Status}'."
            }, "Task status updated successfully.");
        }
    }
}
