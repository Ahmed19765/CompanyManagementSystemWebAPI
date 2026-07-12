using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Domain.Entities
{
    public class CompanyOffers
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public decimal OfferedBudget { get; set; } // The budget offered by the company (Owner) for the project
        public DateTime StartDate { get; set; } // The date when the work on project will start
        public DateTime DeliveryExceptedDate { get; set; } // The date when the work on project will end
        public OfferStatus Status { get; set; } // e.g., "Pending", "Accepted", "Rejected"


    }
}
