using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, CreateDepartmentResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CreateDepartmentCommandHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<CreateDepartmentResponse> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
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
                throw new Exception("Only owners can create departments.");
            }

            var company = await _companyRepository.GetByCompanyNameAsync(request.CompanyName);
            if (company is null)
            {
                throw new Exception("Company not found.");
            }

            if (company.OwnerId != owner.Id)
            {
                throw new Exception("You can only create departments for your own company.");
            }

            var departmentExists = await _departmentRepository.ExistsByNameInCompanyAsync(
                request.CompanyName,
                request.DepartmentName);

            if (departmentExists)
            {
                throw new Exception("Department already exists in this company.");
            }

            var Cid = await _companyRepository.GetCompanyIdFromNameAsync(request.CompanyName);

            if (Cid is null)
            {
                throw new Exception("Company Not Exist!!");
            }

            var department = new Department
            {
                CompanyId =  Cid.Value,
                DepartmentName = request.DepartmentName
            };

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            return new CreateDepartmentResponse
            {
                DepartmentId = department.DepartmentId,
                Message = "Department created successfully."
            };
        }
    }
}
