using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteTeam
{
    public class DeleteTeamCommandHandler
        : IRequestHandler<DeleteTeamCommand, Response<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public DeleteTeamCommandHandler(
            IUserRepository userRepository,
            ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public async Task<Response<string>> Handle(
            DeleteTeamCommand request,
            CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("Owner not found.");

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

            await _teamRepository.DeleteAsync(request.TeamId);

            return Response<string>.Ok(null!, "Team deleted successfully.");
        }
    }
}
