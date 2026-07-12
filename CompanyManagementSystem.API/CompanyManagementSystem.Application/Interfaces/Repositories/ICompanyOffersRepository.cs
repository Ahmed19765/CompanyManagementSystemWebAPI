using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ICompanyOffersRepository
    {
        /// <summary>Returns an offer by its composite key (CompanyId + ProjectId).</summary>
        Task<CompanyOffers?> GetByIdAsync(Guid companyId, Guid projectId);

        /// <summary>Returns true if the company already has an offer on this project.</summary>
        Task<bool> ExistsAsync(Guid companyId, Guid projectId);

        /// <summary>Returns all offers (with Company loaded) for a given project.</summary>
        Task<IEnumerable<CompanyOffers>> GetAllByProjectIdAsync(Guid projectId);

        /// <summary>
        /// Sets the chosen offer to Accepted and all other offers on the same
        /// project to Rejected in a single round-trip.
        /// </summary>
        Task AcceptOfferAndRejectOthersAsync(Guid chosenCompanyId, Guid projectId);

        /// <summary>Adds a new offer.</summary>
        Task AddAsync(CompanyOffers offer);

        Task SaveChangesAsync();
    }
}
