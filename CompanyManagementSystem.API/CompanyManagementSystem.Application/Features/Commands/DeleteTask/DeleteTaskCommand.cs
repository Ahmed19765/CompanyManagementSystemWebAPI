using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Response<string>>
    {
        public Guid TaskId { get; set; }

        [JsonIgnore]
        public Guid CurrentUserId { get; set; }
    }
}
