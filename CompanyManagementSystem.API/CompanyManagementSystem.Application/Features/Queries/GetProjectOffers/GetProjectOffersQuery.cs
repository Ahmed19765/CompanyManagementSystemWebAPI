using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Queries.GetProjectOffers
{
    public class GetProjectOffersQuery : IRequest<Response<GetProjectOffersResponse>>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        // Sent from the client in the JSON body
        public Guid ProjectId { get; set; }
    }
}
