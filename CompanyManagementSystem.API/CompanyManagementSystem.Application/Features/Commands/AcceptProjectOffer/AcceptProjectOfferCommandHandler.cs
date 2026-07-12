using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferCommandHandler
        : IRequestHandler<AcceptProjectOfferCommand, Response<AcceptProjectOfferResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyOffersRepository _companyOffersRepository;

        public AcceptProjectOfferCommandHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ICompanyOffersRepository companyOffersRepository)
        {
            _userRepository          = userRepository;
            _projectRepository       = projectRepository;
            _companyOffersRepository = companyOffersRepository;
        }

        public async Task<Response<AcceptProjectOfferResponse>> Handle(
            AcceptProjectOfferCommand request,
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

            // ── 2. Validate the project belongs to this customer ──────────────────
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                throw new Exception("Project not found.");

            if (project.CustomerId != request.CustomerId)
                throw new Exception("Access denied. This is not your project.");

            // ── 3. Project must still be Pending ──────────────────────────────────
            // Once a project is InProgress or beyond, offers are already resolved.
            if (project.ProjectStatus != ProjectState.Pending)
                throw new Exception(
                    "This project is not in a state that allows accepting offers. " +
                    "Only Pending projects can have an offer accepted.");

            // ── 4. Validate the chosen offer exists ───────────────────────────────
            var chosenOffer = await _companyOffersRepository.GetByIdAsync(
                request.ChosenCompanyId,
                request.ProjectId);

            if (chosenOffer is null)
                throw new Exception("The selected company has not made an offer on this project.");

            if (chosenOffer.Status == OfferStatus.Canceled)
                throw new Exception("This offer has been canceled and cannot be accepted.");

            if (chosenOffer.Status == OfferStatus.Rejected)
                throw new Exception("This offer has already been rejected.");

            if (chosenOffer.Status == OfferStatus.Accepted)
                throw new Exception("This offer is already accepted.");

            // ── 5. Accept chosen offer, reject all others ─────────────────────────
            await _companyOffersRepository.AcceptOfferAndRejectOthersAsync(
                request.ChosenCompanyId,
                request.ProjectId);

            // ── 6. Move project status to InProgress ──────────────────────────────
            project.ProjectStatus = ProjectState.InProgress;
            await _projectRepository.UpdateAsync(project);

            return Response<AcceptProjectOfferResponse>.Ok(new AcceptProjectOfferResponse
            {
                ProjectId           = project.ProjectId,
                ProjectTitle        = project.ProjectTitle ?? string.Empty,
                AcceptedCompanyName = chosenOffer.Company?.CompanyName ?? string.Empty,
                Message             = $"Offer accepted. Your project is now handled by the selected company. All other offers have been rejected."
            }, "Offer accepted successfully.");
        }
    }
}
