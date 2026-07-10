using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects
{
    public class GetCustomerProjectsQueryHandler
        : IRequestHandler<GetCustomerProjectsQuery, GetCustomerProjectsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public GetCustomerProjectsQueryHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<GetCustomerProjectsResponse> Handle(
            GetCustomerProjectsQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
                throw new Exception("User not found.");

            if (customer.IsBanned)
                throw new Exception("This account is banned.");

            if (!customer.EmailConfirmed)
                throw new Exception("Please verify your email.");

            // CustomerId in query comes directly from JWT — customer only ever sees their own projects
            var projects = await _projectRepository.GetAllByCustomerIdAsync(request.CustomerId);

            var dtos = projects.Select(p => new CustomerProjectDto
            {
                ProjectId          = p.ProjectId,
                ProjectTitle       = p.ProjectTitle ?? string.Empty,
                ProjectDescription = p.ProjectDescription,
                OfferedBudget      = p.ProjectOfferedBudget,
                UploadedDate       = p.UploadedDate,

                Offers = p.CompanyOffers.Select(co => new ProjectOfferDto
                {
                    CompanyName            = co.Company?.CompanyName ?? string.Empty,
                    OfferedBudget          = co.OfferedBudget,
                    StartDate              = co.StartDate,
                    DeliveryExpectedDate   = co.DeliveryExceptedDate,
                    Status                 = co.Status.ToString()
                })
            });

            return new GetCustomerProjectsResponse { Projects = dtos };
        }
    }
}
