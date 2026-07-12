using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<Response<CreateDepartmentResponse>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        public string? CompanyName { get; set; }
        public string DepartmentName { get; set; } = null!;
    }
}
