using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer
{
    public class AddCompanyOfferCommand : IRequest<Response<AddCompanyOfferResponse>>
    {
        // Injected from JWT — never from the body
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public Guid CompanyId { get; set; }       // which company is making the offer
        public Guid ProjectId { get; set; }        // which project this offer is for
        public decimal OfferedBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryExpectedDate { get; set; }
    }
}
