using CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetProjectsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        public GetProjectsQueryHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository
            ) 
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<GetProjectsResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            
            var Owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (Owner is null)
                throw new Exception("User not found.");

            if (Owner.IsBanned)
                throw new Exception("This account is banned.");

            if (!Owner.EmailConfirmed)
                throw new Exception("Please verify your email.");
            
            var projects = await _projectRepository.GetAllProjectsAsync();
            var pendingProjects = projects.Where(p => p.ProjectStatus == ProjectState.Pending)
                .Select(p => new ProjectsDto
                {
                    ProjectId = p.ProjectId,
                    ProjectTitle = p.ProjectTitle ?? string.Empty,
                    ProjectDescription = p.ProjectDescription,
                    OfferedBudget = p.ProjectOfferedBudget,
                    UploadedDate = p.UploadedDate,
                    ProjectState = p.ProjectStatus.ToString()
                }
                )
                .ToList();


            return new GetProjectsResponse { Projects = pendingProjects };
        }
    }
}
