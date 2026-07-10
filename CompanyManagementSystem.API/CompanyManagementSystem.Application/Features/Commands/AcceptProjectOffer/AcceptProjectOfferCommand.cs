using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferCommand : IRequest<AcceptProjectOfferResponse>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        // Sent from the client in the JSON body
        public int ProjectId { get; set; }
        public int ChosenCompanyId { get; set; }   // the company the customer wants to handle their project
    }
}
