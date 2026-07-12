using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler
        : IRequestHandler<DeleteTaskCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(
            IUserRepository userRepository,
            ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Response<string>> Handle(
            DeleteTaskCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.CurrentUserId);
            if (user is null)
                throw new Exception("User not found.");

            if (user.IsBanned)
                throw new Exception("Your account is banned.");

            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task is null)
                throw new Exception("Task not found.");

            if (user.Role == UserRole.Owner)
            {
                var companyId = task.Team?.Department?.CompanyId;
                if (companyId is null)
                    throw new Exception("Task is not associated with any company.");

                if (task.Team!.Department!.Company!.OwnerId != request.CurrentUserId)
                    throw new Exception("Access denied. You are not the owner of this company.");
            }
            else if (user.Role == UserRole.Engineer)
            {
                var isTeamLeader = task.Team?.UserTeams
                    .Any(ut => ut.UserId == request.CurrentUserId && ut.TeamRole == "Team Leader") ?? false;

                if (!isTeamLeader)
                    throw new Exception("Access denied. Only the team leader can delete this task.");
            }
            else
            {
                throw new Exception("Access denied. You do not have permission to delete tasks.");
            }

            await _taskRepository.DeleteAsync(request.TaskId);

            return Response<string>.Ok(null!, "Task deleted successfully.");
        }
    }
}
