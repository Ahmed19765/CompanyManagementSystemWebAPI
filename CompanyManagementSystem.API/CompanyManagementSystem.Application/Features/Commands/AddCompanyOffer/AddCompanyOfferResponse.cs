namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyOffer
{
    public class AddCompanyOfferResponse
    {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal OfferedBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryExpectedDate { get; set; }
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
