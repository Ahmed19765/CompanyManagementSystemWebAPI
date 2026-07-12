namespace CompanyManagementSystem.Application.Features.Commands.AcceptProjectOffer
{
    public class AcceptProjectOfferResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string AcceptedCompanyName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
