using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjectOffers
{
    public class GetProjectOffersQueryHandler
        : IRequestHandler<GetProjectOffersQuery, Response<GetProjectOffersResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyOffersRepository _companyOffersRepository;

        public GetProjectOffersQueryHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ICompanyOffersRepository companyOffersRepository)
        {
            _userRepository          = userRepository;
            _projectRepository       = projectRepository;
            _companyOffersRepository = companyOffersRepository;
        }

        public async Task<Response<GetProjectOffersResponse>> Handle(
            GetProjectOffersQuery request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the customer ───────────────────────────────────────────
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
                throw new Exception("User not found.");

            if (customer.IsBanned)
                throw new Exception("Your account is banned.");

            // ── 2. Validate project belongs to this customer ───────────────────────
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                throw new Exception("Project not found.");

            if (project.CustomerId != request.CustomerId)
                throw new Exception("Access denied. This is not your project.");

            // ── 3. Return all offers ───────────────────────────────────────────────
            var offers = await _companyOffersRepository.GetAllByProjectIdAsync(request.ProjectId);

            var dtos = offers.Select(co => new OfferDto
            {
                CompanyId            = co.CompanyId,
                CompanyName          = co.Company?.CompanyName ?? string.Empty,
                CompanyDescription   = co.Company?.CompanyDescription,
                OfferedBudget        = co.OfferedBudget,
                StartDate            = co.StartDate,
                DeliveryExpectedDate = co.DeliveryExceptedDate,
                Status               = co.Status.ToString()
            });

            return Response<GetProjectOffersResponse>.Ok(new GetProjectOffersResponse
            {
                ProjectId    = project.ProjectId,
                ProjectTitle = project.ProjectTitle ?? string.Empty,
                Offers       = dtos
            }, "Offers retrieved successfully.");
        }
    }
}
