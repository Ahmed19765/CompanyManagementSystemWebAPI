using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetDepartmentTeams
{
    public class GetDepartmentTeamsQueryHandler
        : IRequestHandler<GetDepartmentTeamsQuery, GetDepartmentTeamsResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITeamRepository _teamRepository;

        public GetDepartmentTeamsQueryHandler(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _teamRepository = teamRepository;
        }

        public async Task<GetDepartmentTeamsResponse> Handle(
            GetDepartmentTeamsQuery request,
            CancellationToken cancellationToken)
        {
            var requester = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requester is null)
                throw new Exception("User not found.");

            if (requester.IsBanned)
                throw new Exception("This account is banned.");

            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department is null)
                throw new Exception("Department not found.");

            // Only the Owner of the company that owns this department can see its teams
            if (department.Company?.OwnerId != request.RequestingUserId)
                throw new Exception("Access denied. You are not the owner of this company.");

            var teams = await _teamRepository.GetAllByDepartmentIdAsync(request.DepartmentId);

            var dtos = teams.Select(t => new TeamSummaryDto
            {
                TeamId          = t.TeamId,
                TeamName        = t.TeamName ?? string.Empty,
                TeamDescription = t.TeamDescription,
                LeaderUserName  = t.Leader?.UserName,
                MemberCount     = t.UserTeams.Count
            });

            return new GetDepartmentTeamsResponse
            {
                DepartmentId   = request.DepartmentId,
                DepartmentName = department.DepartmentName ?? string.Empty,
                DepartmentDescription = department.DepartmentDescription,
                Teams          = dtos
            };
        }
    }
}
