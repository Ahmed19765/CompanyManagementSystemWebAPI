using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetTeamMembers
{
    public class GetTeamMembersQueryHandler
        : IRequestHandler<GetTeamMembersQuery, GetTeamMembersResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public GetTeamMembersQueryHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public async Task<GetTeamMembersResponse> Handle(
            GetTeamMembersQuery request,
            CancellationToken cancellationToken)
        {
            var requester = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requester is null)
                throw new Exception("User not found.");

            if (requester.IsBanned)
                throw new Exception("This account is banned.");

            var team = await _teamRepository.GetWithMembersAsync(request.TeamId);
            if (team is null)
                throw new Exception("Team not found.");

            // Access rule: Owner of the company OR the Leader of this team
            bool isCompanyOwner = team.Department?.Company?.OwnerId == request.RequestingUserId;
            bool isTeamLeader   = team.LeaderId == request.RequestingUserId;

            if (!isCompanyOwner && !isTeamLeader)
                throw new Exception("Access denied. Only the company owner or team leader can view team members.");

            var dtos = team.UserTeams.Select(ut => new TeamMemberDto
            {
                UserId    = ut.UserId,
                UserName  = ut.User.UserName ?? string.Empty,
                FirstName = ut.User.FirstName,
                LastName  = ut.User.LastName,
                TeamRole  = ut.TeamRole,
                JoinedAt  = ut.JoinedAt
            });

            return new GetTeamMembersResponse
            {
                TeamId   = team.TeamId,
                TeamName = team.TeamName ?? string.Empty,
                Members  = dtos
            };
        }
    }
}
