using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Response<CreateTeamResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public CreateTeamCommandHandler(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            ITeamRepository teamRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _teamRepository = teamRepository;
            _companyUserRepository = companyUserRepository;
        }

        public async Task<Response<CreateTeamResponse>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
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
                throw new Exception("Only owners can create teams.");
            }

            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department is null)
            {
                throw new Exception("Department not found.");
            }

            if (department.Company?.OwnerId != owner.Id)
            {
                throw new Exception("You can only create teams for departments in your own company.");
            }

            if (request.LeaderUserName is null)
            {
                throw new Exception("This user name is currently not exist!");
            }


            var leader = await _userRepository.GetByUserNameAsync(request.LeaderUserName);

            if (leader is null)
            {
                throw new Exception("Team leader not found.");
            }

            if (leader.IsBanned)
            {
                throw new Exception("Team leader account is banned.");
            }

            if (leader.Role != UserRole.Engineer)
            {
                throw new Exception("Team leader must have Engineer role.");
            }

            var leaderMembership = await _companyUserRepository.GetMembershipAsync(
                department.CompanyId ?? Guid.Empty,
                leader.Id);

            if (leaderMembership is null)
            {
                throw new Exception("Team leader must belong to the same company as the department.");
            }

            if (leaderMembership.Rank != CompanyRank.Leader)
            {
                throw new Exception("Team leader must have Leader rank in this company.");
            }

            var teamExists = await _teamRepository.ExistsByNameInDepartmentAsync(
                request.DepartmentId,
                request.TeamName);

            if (teamExists)
            {
                throw new Exception("Team already exists in this department.");
            }

            var team = new Team
            {
                DepartmentId = request.DepartmentId,
                LeaderId = leader.Id,
                TeamName = request.TeamName,
                TeamDescription = request.TeamDescription,
                UserTeams =
                {
                    new UserTeam
                    {
                        UserId = leader.Id,
                        TeamRole = "Team Leader"
                    }
                }
            };

            await _teamRepository.AddAsync(team);
            await _teamRepository.SaveChangesAsync();

            return Response<CreateTeamResponse>.Ok(
                new CreateTeamResponse
                {
                    TeamId = team.TeamId,
                    Message = "Team created successfully."
                },
                "Team created successfully.");
        }
    }
}
