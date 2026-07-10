using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer
{
    public class AddCompanyOfferCommand : IRequest<AddCompanyOfferResponse>
    {
        // Injected from JWT — never from the body
        [JsonIgnore]
        public Guid OwnerId { get; set; }

        // Sent from the client in the JSON body
        public int CompanyId { get; set; }       // which company is making the offer
        public int ProjectId { get; set; }        // which project this offer is for
        public decimal OfferedBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryExpectedDate { get; set; }
    }
}
