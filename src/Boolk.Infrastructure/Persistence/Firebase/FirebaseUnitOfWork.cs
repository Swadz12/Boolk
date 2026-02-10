using Boolk.Domain.Factories;
using Boolk.Application.Interfaces;
using Google.Cloud.Firestore;

namespace Boolk.Infrastructure.Persistence.Firebase;

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
        return Task.FromResult(1);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
