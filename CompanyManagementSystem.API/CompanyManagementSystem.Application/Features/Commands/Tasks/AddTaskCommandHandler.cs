using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.Tasks
{
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, Response<AddTaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public AddTaskCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            IProjectRepository projectRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Response<AddTaskResponse>> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. Get creator info
            var creator = await _userRepository.GetByIdAsync(request.CurrentUserId);

            if (creator == null)
            {
                throw new Exception("Current user (creator) not found.");
            }

            // 2. Get Project and Team info
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new Exception("Project not found.");
            }

            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team == null)
            {
                throw new Exception("Team not found.");
            }

            if (team.Department is null)
            {
                throw new Exception("Team department not found.");
            }

            var companyId = team.Department.CompanyId ?? Guid.Empty;
            var creatorOwnsCompany = team.Department.Company?.OwnerId == creator.Id;
            var creatorMembership = await _companyUserRepository.GetMembershipAsync(companyId, creator.Id);

            if (!creatorOwnsCompany && creatorMembership is null)
            {
                throw new Exception("You are not a member of this company.");
            }

            /*
            if (!creatorOwnsCompany && creatorMembership?.Rank != CompanyRank.Leader)
            {
                throw new Exception("Only company owners or leaders can add/assign tasks.");
            }
            */

            // 3. Validate AssignedTo user if provided
            User? assignedToUser = null;
            CompanyUser? assignedToMembership = null;
            if (request.AssignedToId.HasValue)
            {
                assignedToUser = await _userRepository.GetByIdAsync(request.AssignedToId.Value);
                if (assignedToUser == null)
                {
                    throw new Exception("Assigned user not found.");
                }

                if (assignedToUser.Role != UserRole.Engineer)
                {
                    throw new Exception("Tasks can only be assigned to engineers.");
                }

                assignedToMembership = await _companyUserRepository.GetMembershipAsync(companyId, assignedToUser.Id);
                if (assignedToMembership is null)
                {
                    throw new Exception("Assigned user must belong to the same company as the team.");
                }
            }

            // 4. Apply company-rank checks
            if (creatorOwnsCompany)
            {
                // Company owners can assign tasks to members of their company.
            }
            else if (creatorMembership?.Rank == CompanyRank.Leader)
            {
                // Leader must lead the specified team
                if (team.LeaderId != creator.Id)
                {
                    throw new Exception("You can only assign tasks to teams you are leading.");
                }

                // Leader can only assign to members of their team
                if (assignedToUser != null)
                {
                    bool isMember = team.UserTeams.Any(ut => ut.UserId == assignedToUser.Id);
                    if (!isMember)
                    {
                        throw new Exception("Leaders can only assign tasks to members of their team.");
                    }
                }
            }

            // 6. Create Task & Details together!
            // EF Core handles the relationship and sets the generated TaskId on TaskDetails automatically
            var task = new Domain.Entities.Tasks
            {
                TaskTitle = request.Title,
                TaskDescription = request.Description,
                ProjectId = request.ProjectId,
                TeamId = request.TeamId,
                AssignedById = creator.Id,
                AssignedToId = request.AssignedToId,
                Status = TaskState.Todo,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow,
                Details = new TaskDetails
                {
                    Notes = request.Notes,
                    TaskAttachmentDocumentUrl = request.AttachmentUrl,
                    AcceptanceCriteria = request.AcceptanceCriteria
                }
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return Response<AddTaskResponse>.Ok(new AddTaskResponse
            {
                TaskId = task.TaskId,
                Title = task.TaskTitle!,
                Status = task.Status.ToString(),
                Message = "Task created successfully with details!"
            }, "Task created successfully.");
        }
    }
}
