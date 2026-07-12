namespace CompanyManagementSystem.Application.Features.Queries.GetCustomerProjects
{
    public class CustomerProjectDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public string? ProjectDescription { get; set; }
        public decimal OfferedBudget { get; set; }
        public DateTime? UploadedDate { get; set; }

        public IEnumerable<ProjectOfferDto> Offers { get; set; } = new List<ProjectOfferDto>();
    }

    public class ProjectOfferDto
    {
        public string CompanyName { get; set; } = null!;
        public decimal OfferedBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryExpectedDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
