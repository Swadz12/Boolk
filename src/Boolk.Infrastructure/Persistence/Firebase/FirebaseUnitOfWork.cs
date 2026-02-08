using Boolk.Domain.Factories;
using Boolk.Application.Interfaces;
using Google.Cloud.Firestore;

namespace Boolk.Infrastructure.Persistence.Firebase;

/// <summary>
/// Firebase implementation of Unit of Work.
/// Note: Firebase auto-commits, so SaveChangesAsync is mostly for pattern consistency.
/// This enables easy future migration to SQL databases with proper transactions.
/// </summary>
public class FirebaseUnitOfWork : IUnitOfWork
{
    private readonly FirestoreDb _db;
    private readonly RestaurantFactory _factory;
    private IRestaurantRepository? _restaurants;
    private IReviewRepository? _reviews;
    private IUserRepository? _users;

    public FirebaseUnitOfWork(FirestoreDb db, RestaurantFactory factory)
    {
        _db = db;
        _factory = factory;
    }

    public IRestaurantRepository Restaurants 
        => _restaurants ??= new FirebaseRestaurantRepository(_db, _factory);

    public IReviewRepository Reviews 
        => _reviews ??= new FirebaseReviewRepository(_db);

    public IUserRepository Users 
        => _users ??= new FirebaseUserRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Firebase auto-commits on each operation
        // This method exists for pattern consistency with SQL databases
        return Task.FromResult(1);
    }

    public void Dispose()
    {
        // No resources to dispose for Firebase
        GC.SuppressFinalize(this);
    }
}
