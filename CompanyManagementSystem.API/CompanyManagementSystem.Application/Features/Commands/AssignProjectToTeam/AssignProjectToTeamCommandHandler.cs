using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamCommandHandler
        : IRequestHandler<AssignProjectToTeamCommand, AssignProjectToTeamResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompanyOffersRepository _companyOffersRepository;
        private readonly IProjectTeamRepository _projectTeamRepository;

        public AssignProjectToTeamCommandHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            ICompanyOffersRepository companyOffersRepository,
            IProjectTeamRepository projectTeamRepository)
        {
            _userRepository          = userRepository;
            _projectRepository       = projectRepository;
            _teamRepository          = teamRepository;
            _companyOffersRepository = companyOffersRepository;
            _projectTeamRepository   = projectTeamRepository;
        }

        public async Task<AssignProjectToTeamResponse> Handle(
            AssignProjectToTeamCommand request,
            CancellationToken cancellationToken)
        {
            // ── 1. Validate the owner ──────────────────────────────────────────────
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner is null)
                throw new Exception("User not found.");

            if (owner.IsBanned)
                throw new Exception("Your account is banned.");

            if (!owner.EmailConfirmed)
                throw new Exception("Please verify your email.");

            if (owner.Role != UserRole.Owner)
                throw new Exception("Only owners can assign projects to teams.");

            // ── 2. Load the project ────────────────────────────────────────────────
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project is null)
                throw new Exception("Project not found.");

            // ── 3. Load the team with department → company context ────────────────
            var team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team is null)
                throw new Exception("Team not found.");

            if (team.Department is null)
                throw new Exception("This team is not linked to any department. Please reassign it first.");

            var companyId = team.Department.CompanyId ?? 0;
            if (companyId == 0)
                throw new Exception("This team's department is not linked to any company.");

            // ── 4. Verify the owner owns that company ──────────────────────────────
            if (team.Department.Company?.OwnerId != request.OwnerId)
                throw new Exception("Access denied. This team does not belong to your company.");

            // ── 5. The project must have an Accepted offer from this owner's company ─
            // This ensures you only assign projects that your company committed to handle,
            // not random projects from other companies.
            var acceptedOffer = await _companyOffersRepository.GetByIdAsync(companyId, request.ProjectId);

            if (acceptedOffer is null || acceptedOffer.Status != OfferStatus.Accepted)
                throw new Exception(
                    "Your company does not have an accepted offer on this project. " +
                    "Only projects your company has committed to handle can be assigned to your teams.");

            // ── 6. Prevent duplicate assignment ───────────────────────────────────
            var alreadyAssigned = await _projectTeamRepository.IsAlreadyAssignedAsync(
                request.ProjectId,
                request.TeamId);

            if (alreadyAssigned)
                throw new Exception("This team is already assigned to this project.");

            // ── 7. Assign ──────────────────────────────────────────────────────────
            await _projectTeamRepository.AssignAsync(request.ProjectId, request.TeamId);
            await _projectTeamRepository.SaveChangesAsync();

            return new AssignProjectToTeamResponse
            {
                ProjectId    = project.ProjectId,
                ProjectTitle = project.ProjectTitle ?? string.Empty,
                TeamId       = team.TeamId,
                TeamName     = team.TeamName ?? string.Empty,
                Message      = $"Project '{project.ProjectTitle}' successfully assigned to team '{team.TeamName}'."
            };
        }
    }
}
