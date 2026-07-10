using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<UpdateProjectResponse>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        // Sent from the client in the JSON body
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public string? ProjectDocumentsUrl { get; set; }
        public decimal ProjectOfferedBudget { get; set; }
    }
}
