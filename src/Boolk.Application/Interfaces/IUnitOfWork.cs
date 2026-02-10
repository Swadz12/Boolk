namespace Boolk.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRestaurantRepository Restaurants { get; }
    IReviewRepository Reviews { get; }
    IUserRepository Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
