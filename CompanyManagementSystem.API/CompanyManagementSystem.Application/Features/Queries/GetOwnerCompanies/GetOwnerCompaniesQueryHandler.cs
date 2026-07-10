using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetOwnerCompanies
{
    public class GetOwnerCompaniesQueryHandler
        : IRequestHandler<GetOwnerCompaniesQuery, GetOwnerCompaniesResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;

        public GetOwnerCompaniesQueryHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<GetOwnerCompaniesResponse> Handle(
            GetOwnerCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("User not found.");

            if (owner.IsBanned)
                throw new Exception("This account is banned.");

            if (!owner.EmailConfirmed)
                throw new Exception("Please verify your email.");

            var companies = await _companyRepository.GetAllByOwnerIdAsync(request.OwnerId);

            // Map to DTO — one responsibility: project only what the caller needs
            var dtos = companies.Select(c => new CompanyDto
            {
                CompanyId      = c.CompanyId,
                CompanyName    = c.CompanyName ?? string.Empty,
                CompanyDescription = c.CompanyDescription,
                JoinCode       = c.JoinCode,
                MemberCount    = c.CompanyUsers.Count,
                DepartmentCount = c.Departments.Count
            });

            return new GetOwnerCompaniesResponse { Companies = dtos };
        }
    }
}
