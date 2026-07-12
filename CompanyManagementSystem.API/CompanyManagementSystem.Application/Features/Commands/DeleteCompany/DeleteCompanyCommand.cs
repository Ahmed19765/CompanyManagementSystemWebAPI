using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.DeleteCompany
{
    public class DeleteCompanyCommand : IRequest<Response<DeleteCompanyResponse>>
    {
        // Injected from JWT — never from the request body
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public Guid CompanyId { get; set; }
    }
}
