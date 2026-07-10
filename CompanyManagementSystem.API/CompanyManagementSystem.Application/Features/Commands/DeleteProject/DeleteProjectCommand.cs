using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<DeleteProjectResponse>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        // Sent from the client in the JSON body
        public int ProjectId { get; set; }
    }
}
