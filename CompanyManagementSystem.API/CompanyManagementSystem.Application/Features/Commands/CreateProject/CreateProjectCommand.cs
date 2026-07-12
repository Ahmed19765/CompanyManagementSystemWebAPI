using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<Response<CreateProjectResponse>>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        public string ProjectTitle { get; set; } = null!;
        public string ProjectDescription { get; set; } = null!;
        public string? ProjectDocumentsUrl { get; set; }
        public decimal ProjectOfferedBudget { get; set; }
    }
}
