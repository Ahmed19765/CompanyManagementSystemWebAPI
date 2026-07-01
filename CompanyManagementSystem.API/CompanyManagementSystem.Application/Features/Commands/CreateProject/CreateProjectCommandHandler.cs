using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var customer = await _userRepository.GetByIdAsync(request.CustomerId);
            if (customer is null)
            {
                throw new Exception("User not found.");
            }

            if (customer.IsBanned)
            {
                throw new Exception("This account is banned!");
            }

            if (!customer.IsEmailVerfied)
            {
                throw new Exception("Please verfiey your email!");
            }

            if (customer.Role != UserRole.Customer)
            {
                throw new Exception("Only customers can create projects.");
            }

            var project = new Project
            {
                ProjectTitle = request.ProjectTitle,
                ProjectDescription = request.ProjectDescription,
                ProjectDocumentsUrl = request.ProjectDocumentsUrl,
                ProjectOfferedBudget = request.ProjectOfferedBudget,
                UploadedDate = DateTime.UtcNow,
                CustomerId = customer.UserId
            };

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            return new CreateProjectResponse
            {
                ProjectId = project.ProjectId,
                Message = "Project created successfully."
            };
        }
    }
}
