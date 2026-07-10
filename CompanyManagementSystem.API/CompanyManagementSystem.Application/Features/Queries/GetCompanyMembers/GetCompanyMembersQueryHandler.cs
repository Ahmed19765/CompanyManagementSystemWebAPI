using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyMembers
{
    public class GetCompanyMembersQueryHandler
        : IRequestHandler<GetCompanyMembersQuery, GetCompanyMembersResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public GetCompanyMembersQueryHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<GetCompanyMembersResponse> Handle(
            GetCompanyMembersQuery request,
            CancellationToken cancellationToken)
        {
            var requester = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requester is null)
                throw new Exception("User not found.");

            if (requester.IsBanned)
                throw new Exception("This account is banned.");

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            // Only the Owner of this specific company can see its members
            if (company.OwnerId != request.RequestingUserId)
                throw new Exception("Access denied. You are not the owner of this company.");

            var memberships = await _companyUserRepository.GetAllMembersByCompanyIdAsync(request.CompanyId);

            var dtos = memberships.Select(cu => new CompanyMemberDto
            {
                UserId    = cu.UserId,
                UserName  = cu.User.UserName ?? string.Empty,
                FirstName = cu.User.FirstName,
                LastName  = cu.User.LastName,
                Email     = cu.User.Email,
                Rank      = cu.Rank.ToString(),
                JoinedAt  = cu.JoinedAt
            });

            return new GetCompanyMembersResponse
            {
                CompanyId = request.CompanyId,
                Members   = dtos
            };
        }
    }
}
