using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyAcceptedProjects
{
    public class GetCompanyAcceptedProjectsQueryHandler
        : IRequestHandler<GetCompanyAcceptedProjectsQuery, Response<GetCompanyAcceptedProjectsResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectRepository _projectRepository;

        public GetCompanyAcceptedProjectsQueryHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository)
        {
            _userRepository    = userRepository;
            _companyRepository = companyRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Response<GetCompanyAcceptedProjectsResponse>> Handle(
            GetCompanyAcceptedProjectsQuery request,
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

            // ── 2. Validate the company belongs to this owner ─────────────────────
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            if (company.OwnerId != request.OwnerId)
                throw new Exception("Access denied. This company does not belong to you.");

            // ── 3. Fetch all projects this company has an accepted offer on ────────
            var projects = await _projectRepository
                .GetAcceptedProjectsByCompanyIdAsync(request.CompanyId);

            // ── 4. Map to DTO — calculate progress per project ────────────────────
            var dtos = projects.Select(p =>
            {
                int total = p.Tasks.Count;
                int done  = p.Tasks.Count(t => t.Status == TaskState.Done);

                // Progress = 0% if no tasks yet (fresh / newly accepted project)
                int progress = total > 0
                    ? (int)Math.Round((double)done / total * 100)
                    : 0;

                return new AcceptedProjectDto
                {
                    ProjectId          = p.ProjectId,
                    ProjectTitle       = p.ProjectTitle ?? string.Empty,
                    ProjectDescription = p.ProjectDescription,
                    ProjectStatus      = p.ProjectStatus?.ToString() ?? string.Empty,
                    ProgressPercent    = progress,
                    TotalTasks         = total,
                    DoneTasks          = done
                };
            });

            var response = new GetCompanyAcceptedProjectsResponse
            {
                CompanyId   = company.CompanyId,
                CompanyName = company.CompanyName ?? string.Empty,
                Projects    = dtos
            };

            return Response<GetCompanyAcceptedProjectsResponse>.Ok(response, "Projects retrieved successfully.");
        }
    }
}
