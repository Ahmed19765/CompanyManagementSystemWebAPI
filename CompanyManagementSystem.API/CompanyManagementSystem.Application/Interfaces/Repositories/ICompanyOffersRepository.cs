using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ICompanyOffersRepository
    {
        /// <summary>Returns an offer by its composite key (CompanyId + ProjectId).</summary>
        Task<CompanyOffers?> GetByIdAsync(int companyId, int projectId);

        /// <summary>Returns true if the company already has an offer on this project.</summary>
        Task<bool> ExistsAsync(int companyId, int projectId);

        /// <summary>Returns all offers (with Company loaded) for a given project.</summary>
        Task<IEnumerable<CompanyOffers>> GetAllByProjectIdAsync(int projectId);

        /// <summary>
        /// Sets the chosen offer to Accepted and all other offers on the same
        /// project to Rejected in a single round-trip.
        /// </summary>
        Task AcceptOfferAndRejectOthersAsync(int chosenCompanyId, int projectId);

        /// <summary>Adds a new offer.</summary>
        Task AddAsync(CompanyOffers offer);

        Task SaveChangesAsync();
    }
}
