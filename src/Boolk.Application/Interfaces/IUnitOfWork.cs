namespace Boolk.Application.Interfaces;

/// <summary>
/// Unit of Work pattern interface.
/// Groups multiple repository operations into a single transaction.
/// Provides a single point of access to all repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRestaurantRepository Restaurants { get; }
    IReviewRepository Reviews { get; }
    IUserRepository Users { get; }
    
    /// <summary>
    /// Commits all changes made through the repositories.
    /// For Firebase, this is a no-op since Firebase auto-commits.
    /// For SQL databases, this would commit the transaction.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
