using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberCommandHandler
        : IRequestHandler<RemoveTeamMemberCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public RemoveTeamMemberCommandHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public async Task<Response<string>> Handle(
            RemoveTeamMemberCommand request,
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

            var userTeam = team.UserTeams.FirstOrDefault(ut => ut.UserId == request.UserId);
            if (userTeam is null)
                throw new Exception("User is not a member of this team.");

            if (team.LeaderId == request.UserId)
            {
                team.LeaderId = null;
            }

            team.UserTeams.Remove(userTeam);
            await _teamRepository.SaveChangesAsync();

            return Response<string>.Ok(null!, "Member removed from team successfully.");
        }
    }
}
