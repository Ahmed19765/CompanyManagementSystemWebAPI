using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Application.Interfaces.Repositories;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyDepartments
{
    public class GetCompanyDepartmentsQueryHandler
        : IRequestHandler<GetCompanyDepartmentsQuery, Response<GetCompanyDepartmentsResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public GetCompanyDepartmentsQueryHandler(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Response<GetCompanyDepartmentsResponse>> Handle(
            GetCompanyDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            var requester = await _userRepository.GetByIdAsync(request.RequestingUserId);
            if (requester is null)
                throw new Exception("User not found.");

            if (requester.IsBanned)
                throw new Exception("This account is banned.");

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company is null)
                throw new Exception("Company not found.");

            if (company.OwnerId != request.RequestingUserId)
                throw new Exception("Access denied. You are not the owner of this company.");

            var departments = await _departmentRepository.GetAllByCompanyIdAsync(request.CompanyId);

            var dtos = departments.Select(d => new DepartmentDto
            {
                DepartmentId   = d.DepartmentId,
                DepartmentName = d.DepartmentName ?? string.Empty,
                DepartmentDescription = d.DepartmentDescription,
                TeamCount      = d.Teams.Count
            });

            var response = new GetCompanyDepartmentsResponse
            {
                CompanyId   = request.CompanyId,
                Departments = dtos
            };

            return Response<GetCompanyDepartmentsResponse>.Ok(response, "Departments retrieved successfully.");
        }
    }
}
