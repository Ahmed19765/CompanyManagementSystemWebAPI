using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.AddTeamMember
{
    public class AddTeamMemberCommandHandler
        : IRequestHandler<AddTeamMemberCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public AddTeamMemberCommandHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Response<string>> Handle(
            AddTeamMemberCommand request,
            CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("User not found.");

            if (owner.IsBanned)
                throw new Exception("Your account is banned.");

            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team is null)
                throw new Exception("Team not found.");

            var companyId = team.Department?.CompanyId;
            if (companyId is null)
                throw new Exception("Team is not associated with any company.");

            if (team.Department!.Company!.OwnerId != request.OwnerId)
                throw new Exception("Access denied. You are not the owner of this company.");

            var membership = await _companyUserRepository.GetMembershipAsync(companyId.Value, request.UserId);
            if (membership is null)
                throw new Exception("User is not a member of this company. Only company members can be added to teams.");

            if (team.UserTeams.Any(ut => ut.UserId == request.UserId))
                throw new Exception("User is already a member of this team.");

            var userTeam = new UserTeam
            {
                UserId = request.UserId,
                TeamId = request.TeamId,
                TeamRole = request.TeamRole ?? "Member"
            };

            team.UserTeams.Add(userTeam);
            await _teamRepository.SaveChangesAsync();

            return Response<string>.Ok(null!, "Member added to team successfully.");
        }
    }
}
