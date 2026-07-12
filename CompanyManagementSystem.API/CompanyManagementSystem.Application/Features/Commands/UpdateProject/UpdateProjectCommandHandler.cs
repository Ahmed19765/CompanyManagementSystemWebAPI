using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler
        : IRequestHandler<UpdateProjectCommand, Response<UpdateProjectResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        // The edit/delete window allowed after project creation
        private static readonly TimeSpan EditWindow = TimeSpan.FromDays(3);

        public UpdateProjectCommandHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _userRepository    = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Response<UpdateProjectResponse>> Handle(
            UpdateProjectCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the customer ───────────────────────────────────────────
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
                throw new Exception("User not found.");

            if (customer.IsBanned)
                throw new Exception("Your account is banned.");

            if (!customer.EmailConfirmed)
                throw new Exception("Please verify your email.");

            // ── 2. Load the project ────────────────────────────────────────────────
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                throw new Exception("Project not found.");

            // ── 3. Ownership check ─────────────────────────────────────────────────
            if (project.CustomerId != request.CustomerId)
                throw new Exception("Access denied. This is not your project.");

            // ── 4. 3-day edit window check ─────────────────────────────────────────
            var age = DateTime.UtcNow - (project.UploadedDate ?? DateTime.UtcNow);
            if (age > EditWindow)
                throw new Exception(
                    "Edit window has expired. Projects can only be edited within 3 days of creation.");

            // ── 5. Accepted offer lock ─────────────────────────────────────────────
            // If any company already accepted this project, it is locked — editing
            // would change the terms of an active agreement.
            var hasAcceptedOffer = await _projectRepository.HasAcceptedOfferAsync(request.ProjectId);
            if (hasAcceptedOffer)
                throw new Exception(
                    "This project is locked. A company has already accepted an offer on it.");

            // ── 6. Apply updates ───────────────────────────────────────────────────
            project.ProjectTitle        = request.ProjectTitle;
            project.ProjectDescription  = request.ProjectDescription;
            project.ProjectDocumentsUrl = request.ProjectDocumentsUrl;
            project.ProjectOfferedBudget = request.ProjectOfferedBudget;

            await _projectRepository.UpdateAsync(project);

            return Response<UpdateProjectResponse>.Ok(new UpdateProjectResponse
            {
                ProjectId            = project.ProjectId,
                ProjectTitle         = project.ProjectTitle ?? string.Empty,
                ProjectDescription   = project.ProjectDescription,
                ProjectDocumentsUrl  = project.ProjectDocumentsUrl,
                ProjectOfferedBudget = project.ProjectOfferedBudget,
                Message              = "Project updated successfully."
            }, "Project updated successfully.");
        }
    }
}
