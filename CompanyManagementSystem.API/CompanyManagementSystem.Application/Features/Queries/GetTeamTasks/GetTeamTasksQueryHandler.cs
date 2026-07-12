using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamTasks
{
    public class GetTeamTasksQueryHandler
        : IRequestHandler<GetTeamTasksQuery, Response<GetTeamTasksResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITaskRepository _taskRepository;

        public GetTeamTasksQueryHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Response<GetTeamTasksResponse>> Handle(
            GetTeamTasksQuery request,
            CancellationToken cancellationToken)
        {
            var requester = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requester is null)
                throw new Exception("User not found.");

            if (requester.IsBanned)
                throw new Exception("This account is banned.");

            // GetByIdAsync loads Department → Company and UserTeams
            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team is null)
                throw new Exception("Team not found.");

            // Access rule: Owner of the company OR the Leader of this team
            bool isCompanyOwner = team.Department?.Company?.OwnerId == request.RequestingUserId;
            bool isTeamLeader   = team.LeaderId == request.RequestingUserId;

            if (!isCompanyOwner && !isTeamLeader)
                throw new Exception("Access denied. Only the company owner or team leader can view team tasks.");

            var tasks = await _taskRepository.GetAllByTeamIdAsync(request.TeamId);

            var dtos = tasks.Select(t => new TaskSummaryDto
            {
                TaskId              = t.TaskId,
                TaskTitle           = t.TaskTitle ?? string.Empty,
                TaskDescription     = t.TaskDescription,
                Status              = t.Status.ToString(),
                CreatedAt           = t.CreatedAt,
                DueDate             = t.DueDate,
                AssignedToUserName  = t.AssignedTo?.UserName,
                AssignedByUserName  = t.AssignedBy?.UserName,
                Notes               = t.Details?.Notes,
                AcceptanceCriteria  = t.Details?.AcceptanceCriteria
            });

            return Response<GetTeamTasksResponse>.Ok(
                new GetTeamTasksResponse
                {
                    TeamId   = team.TeamId,
                    TeamName = team.TeamName ?? string.Empty,
                    Tasks    = dtos
                },
                "Tasks retrieved successfully.");
        }
    }
}
