using Boolk.Models;
using Boolk.Repositories.Interfaces;
using Google.Cloud.Firestore;
using Boolk.Firebase;

namespace Boolk.Repositories.Firebase;

public class FirebaseReviewRepository : IReviewRepository
{
    private readonly FirestoreDb _db;
    private const string CollectionName = "reviews";

    public FirebaseReviewRepository()
    {
        _db = FirebaseInitializer.GetFirestoreDb();
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists) return null;
        
        return MapToReview(snapshot);
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        var snapshot = await _db.Collection(CollectionName).GetSnapshotAsync();
        return snapshot.Documents
            .Select(doc => MapToReview(doc))
            .Where(r => r != null)
            .Cast<Review>();
    }

    public async Task<IEnumerable<Review>> GetByRestaurantIdAsync(Guid restaurantId)
    {
        var snapshot = await _db.Collection(CollectionName)
            .WhereEqualTo("RestaurantId", restaurantId.ToString())
            .GetSnapshotAsync();
        
        return snapshot.Documents
            .Select(doc => MapToReview(doc))
            .Where(r => r != null)
            .Cast<Review>();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
    {
        var snapshot = await _db.Collection(CollectionName)
            .WhereEqualTo("UserId", userId.ToString())
            .GetSnapshotAsync();
        
        return snapshot.Documents
            .Select(doc => MapToReview(doc))
            .Where(r => r != null)
            .Cast<Review>();
    }

    public async Task<Review> CreateAsync(Review review)
    {
        if (review.Id == Guid.Empty)
            review.Id = Guid.NewGuid();

        var docRef = _db.Collection(CollectionName).Document(review.Id.ToString());
        await docRef.SetAsync(MapToDictionary(review));
        
        return review;
    }

    public async Task UpdateAsync(Review review)
    {
        var docRef = _db.Collection(CollectionName).Document(review.Id.ToString());
        await docRef.SetAsync(MapToDictionary(review), SetOptions.MergeAll);
    }

    public async Task DeleteAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        await docRef.DeleteAsync();
    }

    private Review? MapToReview(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        if (data == null) return null;

        return new Review
        {
            Id = Guid.Parse(doc.Id),
            UserId = Guid.Parse(data["UserId"].ToString() ?? Guid.Empty.ToString()),
            RestaurantId = Guid.Parse(data["RestaurantId"].ToString() ?? Guid.Empty.ToString()),
            Price = Convert.ToDecimal(data["Price"]),
            SatietyLevel = Convert.ToInt32(data["SatietyLevel"]),
            Comment = data["Comment"].ToString() ?? ""
        };
    }

    private Dictionary<string, object> MapToDictionary(Review review)
    {
        return new Dictionary<string, object>
        {
            { "Id", review.Id.ToString() },
            { "UserId", review.UserId.ToString() },
            { "RestaurantId", review.RestaurantId.ToString() },
            { "Price", review.Price },
            { "SatietyLevel", review.SatietyLevel },
            { "Comment", review.Comment }
        };
    }
}

