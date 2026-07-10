using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberCommandHandler : IRequestHandler<AddCompanyMemberCommand, AddCompanyMemberResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public AddCompanyMemberCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<AddCompanyMemberResponse> Handle(AddCompanyMemberCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
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
                throw new Exception("Please verify your email!");
            }

            if (owner.Role != UserRole.Owner)
            {
                throw new Exception("Only owners can add company members.");
            }

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            if (company.OwnerId != owner.Id)
            {
                throw new Exception("You can only add members to your own company.");
            }

            var targetUser = await _userRepository.GetByUserNameAsync(request.UserName);
            if (targetUser is null)
            {
                throw new Exception("Target user not found.");
            }

            if (targetUser.IsBanned)
            {
                throw new Exception("Target user account is banned.");
            }

            if (targetUser.Role != UserRole.Engineer)
            {
                throw new Exception("Only engineers can be added as company members.");
            }

            var membership = await _companyUserRepository.GetMembershipAsync(request.CompanyId, targetUser.Id);
            if (membership is not null)
            {
                throw new Exception("User is already a member of this company.");
            }

            var newMembership = new CompanyUser
            {
                CompanyId = request.CompanyId,
                UserId = targetUser.Id,
                Rank = CompanyRank.Member
            };

            await _companyUserRepository.AddAsync(newMembership);
            await _companyUserRepository.SaveChangesAsync();

            return new AddCompanyMemberResponse
            {
                CompanyId = request.CompanyId,
                UserName = request.UserName,
                Message = "Engineer added to the company successfully as a member."
            };
        }
    }
}
