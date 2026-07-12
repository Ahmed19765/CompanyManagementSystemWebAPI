using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest<Response<DeleteDepartmentResponse>>
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public Guid DepartmentId { get; set; }
    }
}
