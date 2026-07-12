using CompanyManagementSystem.Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferCommand : IRequest<Response<AcceptProjectOfferResponse>>
    {
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        // Sent from the client in the JSON body
        public Guid ProjectId { get; set; }
        public Guid ChosenCompanyId { get; set; }   // the company the customer wants to handle their project
    }
}
