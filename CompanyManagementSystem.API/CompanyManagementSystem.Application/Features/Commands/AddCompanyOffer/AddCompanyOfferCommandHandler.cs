using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer
{
    public class AddCompanyOfferCommandHandler
        : IRequestHandler<AddCompanyOfferCommand, AddCompanyOfferResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyOffersRepository _companyOffersRepository;

        public AddCompanyOfferCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository,
            ICompanyOffersRepository companyOffersRepository)
        {
            _userRepository         = userRepository;
            _companyRepository      = companyRepository;
            _projectRepository      = projectRepository;
            _companyOffersRepository = companyOffersRepository;
        }

        public async Task<AddCompanyOfferResponse> Handle(
            AddCompanyOfferCommand request,
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

            if (owner.Role != UserRole.Owner)
                throw new Exception("Only owners can add company offers.");

            // ── 2. Validate the company belongs to this owner ─────────────────────
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            if (company.OwnerId != request.OwnerId)
                throw new Exception("Access denied. This company does not belong to you.");

            if (company.IsDeleted)
                throw new Exception("This company has been deleted and cannot make offers.");

            // ── 3. Validate the project exists and is still Pending ───────────────
            // Only Pending projects are open for offers.
            // InProgress / Completed / Canceled projects are closed.
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                throw new Exception("Project not found.");

            if (project.ProjectStatus != ProjectState.Pending)
                throw new Exception(
                    "This project is not open for offers. Only projects with status 'Pending' can receive offers.");

            // ── 4. Prevent duplicate offer from the same company ──────────────────
            var alreadyOffered = await _companyOffersRepository.ExistsAsync(
                request.CompanyId,
                request.ProjectId);

            if (alreadyOffered)
                throw new Exception("Your company has already submitted an offer for this project.");

            // ── 5. Validate dates ─────────────────────────────────────────────────
            if (request.StartDate < DateTime.UtcNow.Date)
                throw new Exception("Start date cannot be in the past.");

            if (request.DeliveryExpectedDate <= request.StartDate)
                throw new Exception("Delivery date must be after the start date.");

            // ── 6. Create the offer ───────────────────────────────────────────────
            var offer = new CompanyOffers
            {
                CompanyId             = request.CompanyId,
                ProjectId             = request.ProjectId,
                OfferedBudget         = request.OfferedBudget,
                StartDate             = request.StartDate,
                DeliveryExceptedDate  = request.DeliveryExpectedDate,
                Status                = OfferStatus.Pending   // always starts as Pending
            };

            await _companyOffersRepository.AddAsync(offer);
            await _companyOffersRepository.SaveChangesAsync();

            return new AddCompanyOfferResponse
            {
                CompanyId            = offer.CompanyId,
                ProjectId            = offer.ProjectId,
                OfferedBudget        = offer.OfferedBudget,
                StartDate            = offer.StartDate,
                DeliveryExpectedDate = offer.DeliveryExceptedDate,
                Status               = offer.Status.ToString(),
                Message              = "Offer submitted successfully. Waiting for customer response."
            };
        }
    }
}
