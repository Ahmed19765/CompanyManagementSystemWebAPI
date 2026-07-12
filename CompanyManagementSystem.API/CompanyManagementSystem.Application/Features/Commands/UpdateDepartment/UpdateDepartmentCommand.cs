using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<Response<UpdateDepartmentResponse>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;
        public string? DepartmentDescription { get; set; }
    }
}
