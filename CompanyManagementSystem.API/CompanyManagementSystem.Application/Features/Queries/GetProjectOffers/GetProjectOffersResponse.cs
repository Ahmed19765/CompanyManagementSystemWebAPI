namespace CompanyManagementSystem.Application.Features.Queries.GetProjectOffers
{
    public class GetProjectOffersResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public IEnumerable<OfferDto> Offers { get; set; } = new List<OfferDto>();
    }

    public class OfferDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? CompanyDescription { get; set; }
        public decimal OfferedBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryExpectedDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
