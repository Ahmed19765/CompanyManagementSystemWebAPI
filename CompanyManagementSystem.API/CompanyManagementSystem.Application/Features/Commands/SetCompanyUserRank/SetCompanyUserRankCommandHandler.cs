using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.SetCompanyUserRank
{
    public class SetCompanyUserRankCommandHandler : IRequestHandler<SetCompanyUserRankCommand, Response<SetCompanyUserRankResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public SetCompanyUserRankCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Response<SetCompanyUserRankResponse>> Handle(SetCompanyUserRankCommand request, CancellationToken cancellationToken)
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
                throw new Exception("Please verfiey your email!");
            }

            if (owner.Role != UserRole.Owner)
            {
                throw new Exception("Only owners can update company member ranks.");
            }

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            if (company.OwnerId != owner.Id)
            {
                throw new Exception("You can only update ranks for your own company.");
            }

            if (request.UserId == owner.Id)
            {
                throw new Exception("The company owner manages the company through Owner role, not member rank.");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                throw new Exception("Target user not found.");
            }

            if (user.IsBanned)
            {
                throw new Exception("Target user account is banned.");
            }

            if (user.Role != UserRole.Engineer)
            {
                throw new Exception("Only engineers can have a company member rank.");
            }

            var membership = await _companyUserRepository.GetMembershipAsync(request.CompanyId, request.UserId);
            if (membership is null)
            {
                membership = new CompanyUser
                {
                    CompanyId = request.CompanyId,
                    UserId = request.UserId,
                    Rank = request.Rank
                };

                await _companyUserRepository.AddAsync(membership);
            }
            else
            {
                membership.Rank = request.Rank;
                await _companyUserRepository.UpdateAsync(membership);
            }

            await _companyUserRepository.SaveChangesAsync();

            var response = new SetCompanyUserRankResponse
            {
                CompanyId = request.CompanyId,
                UserId = request.UserId,
                Rank = request.Rank,
                Message = "Company member rank updated successfully."
            };

            return Response<SetCompanyUserRankResponse>.Ok(response, "Rank updated successfully.");
        }
    }
}
