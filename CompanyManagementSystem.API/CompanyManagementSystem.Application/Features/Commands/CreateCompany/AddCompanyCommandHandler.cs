using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.CreateCompany
{
    public class CompanyCommandHandler : IRequestHandler<AddCompanyCommand, CompanyCreationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<CompanyCreationResponse> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            var CompanyExist = await _companyRepository.IsCompanyExistWithSameName(request.CompanyName);

            if (owner is null)
            {
                throw new Exception("User not found.");
            }

            if (owner.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            if (!owner.EmailConfirmed)
            {
                throw new Exception("Please verfiey your email!");
            }

            if (owner.Role != UserRole.Owner)
            {
                throw new Exception("Only owners can create companies.");
            }

            if (CompanyExist)
            {
                throw new Exception("Company already exist with that name!");
            }

            var company = new Company
            {
                CompanyName = request.CompanyName,
                CompanyDescription = request.CompanyDescription,
                JoinCode = Guid.NewGuid().ToString(),
                OwnerId = owner.Id
            };

            await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();

            return new CompanyCreationResponse
            {
                Message = "Company created successfully."
            };
        }
    }
}
