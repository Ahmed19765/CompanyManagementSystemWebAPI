using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Response<UpdateDepartmentResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateDepartmentCommandHandler(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Response<UpdateDepartmentResponse>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
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
                throw new Exception("Only owners can update departments.");
            }

            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department is null)
            {
                throw new Exception("Department not found.");
            }

            if (department.Company?.OwnerId != owner.Id)
            {
                throw new Exception("You can only update departments for your own company.");
            }

            var departmentExists = await _departmentRepository.ExistsByNameInCompanyAsync(
                department.CompanyId ?? Guid.Empty,
                request.DepartmentName,
                request.DepartmentId);

            if (departmentExists)
            {
                throw new Exception("Department already exists in this company.");
            }

            department.DepartmentName = request.DepartmentName;
            department.DepartmentDescription = request.DepartmentDescription;

            await _departmentRepository.SaveChangesAsync();

            return Response<UpdateDepartmentResponse>.Ok(
                new UpdateDepartmentResponse
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName,
                    DepartmentDescription = department.DepartmentDescription,
                    Message = "Department updated successfully."
                },
                "Department updated successfully.");
        }
    }
}
