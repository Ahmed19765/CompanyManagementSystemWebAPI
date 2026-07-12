using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler
        : IRequestHandler<DeleteProjectCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        private static readonly TimeSpan EditWindow = TimeSpan.FromDays(3);

        public DeleteProjectCommandHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _userRepository    = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Response<string>> Handle(
            DeleteProjectCommand request,
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

            // ── 4. 3-day delete window check ───────────────────────────────────────
            var age = DateTime.UtcNow - (project.UploadedDate ?? DateTime.UtcNow);
            if (age > EditWindow)
                throw new Exception(
                    "Delete window has expired. Projects can only be deleted within 3 days of creation.");

            // ── 5. Accepted offer lock ─────────────────────────────────────────────
            // If any company already accepted this project it means work is about
            // to start — deleting would break that agreement.
            var hasAcceptedOffer = await _projectRepository.HasAcceptedOfferAsync(request.ProjectId);
            if (hasAcceptedOffer)
                throw new Exception(
                    "This project cannot be deleted. A company has already accepted an offer on it.");

            // ── 6. Hard-delete the project ─────────────────────────────────────────
            await _projectRepository.DeleteAsync(request.ProjectId);

            return Response<string>.Ok(null!, "Project deleted successfully.");
        }
    }
}
