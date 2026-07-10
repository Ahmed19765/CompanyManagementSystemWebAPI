using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetMyAssignedTasks
{
    public class GetMyAssignedTasksQueryHandler
        : IRequestHandler<GetMyAssignedTasksQuery, GetMyAssignedTasksResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public GetMyAssignedTasksQueryHandler(
            IUserRepository userRepository,
            ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<GetMyAssignedTasksResponse> Handle(
            GetMyAssignedTasksQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                throw new Exception("User not found.");

            if (user.IsBanned)
                throw new Exception("This account is banned.");

            if (!user.EmailConfirmed)
                throw new Exception("Please verify your email.");

            // Every authenticated Engineer sees only tasks assigned to them —
            // no extra ownership check needed since UserId comes from their own JWT
            var tasks = await _taskRepository.GetAllAssignedToUserAsync(request.UserId);

            var dtos = tasks.Select(t => new MyTaskDto
            {
                TaskId             = t.TaskId,
                TaskTitle          = t.TaskTitle ?? string.Empty,
                TaskDescription    = t.TaskDescription,
                Status             = t.Status.ToString(),
                CreatedAt          = t.CreatedAt,
                DueDate            = t.DueDate,
                ProjectTitle       = t.Project?.ProjectTitle,
                TeamName           = t.Team?.TeamName,
                Notes              = t.Details?.Notes,
                AcceptanceCriteria = t.Details?.AcceptanceCriteria,
                AttachmentUrl      = t.Details?.TaskAttachmentDocumentUrl
            });

            return new GetMyAssignedTasksResponse { Tasks = dtos };
        }
    }
}
