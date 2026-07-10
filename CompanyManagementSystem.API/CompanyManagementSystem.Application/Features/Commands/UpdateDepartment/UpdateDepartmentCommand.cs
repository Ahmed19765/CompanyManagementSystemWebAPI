using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<UpdateDepartmentResponse>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;
        public string? DepartmentDescription { get; set; }
    }
}
