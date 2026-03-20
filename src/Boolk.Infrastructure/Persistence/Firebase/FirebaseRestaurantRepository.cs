using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Boolk.Domain.Factories;
using Google.Cloud.Firestore;

namespace Boolk.Infrastructure.Persistence.Firebase;

public class FirebaseRestaurantRepository : IRestaurantRepository
{
    private readonly FirestoreDb _db;
    private readonly RestaurantFactory _factory;
    private const string CollectionName = "restaurants";

    public FirebaseRestaurantRepository(FirestoreDb db, RestaurantFactory factory)
    {
        _db = db;
        _factory = factory;
    }

    public async Task<RestaurantBase?> GetByIdAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists) return null;
        
        return MapToRestaurant(snapshot);
    }
    
    public async Task<IEnumerable<RestaurantBase>> GetAllAsync(int skip, int take)
    {
        var snapshot = await _db.Collection(CollectionName)
            .Offset(skip)
            .Limit(take)
            .GetSnapshotAsync();
            
        return snapshot.Documents
            .Select(doc => MapToRestaurant(doc))
            .Where(r => r != null)
            .Cast<RestaurantBase>();
    }

    public async Task<int> GetCountAsync()
    {
        var countQuery = await _db.Collection(CollectionName).Count().GetSnapshotAsync();
        return (int)(countQuery.Count ?? 0);
    }

    public async Task<RestaurantBase> CreateAsync(RestaurantBase restaurant)
    {
        if (restaurant.Id == Guid.Empty)
            restaurant.Id = Guid.NewGuid();

        var docRef = _db.Collection(CollectionName).Document(restaurant.Id.ToString());
        await docRef.SetAsync(MapToDictionary(restaurant));
        
        return restaurant;
    }

    public async Task UpdateAsync(RestaurantBase restaurant)
    {
        var docRef = _db.Collection(CollectionName).Document(restaurant.Id.ToString());
        await docRef.SetAsync(MapToDictionary(restaurant), SetOptions.MergeAll);
    }

    public async Task DeleteAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        await docRef.DeleteAsync();
    }

    private RestaurantBase? MapToRestaurant(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        if (data == null) return null;

        var type = data.ContainsKey("Type") ? data["Type"].ToString() : "";
        var name = data.ContainsKey("Name") ? data["Name"].ToString() : "";
        var city = data.ContainsKey("City") ? data["City"].ToString() : "";
        var id = Guid.Parse(doc.Id);
        
        try
        {
            var restaurant = _factory.Create(type ?? "", name ?? "", city ?? "");
            restaurant.Id = id;
            return restaurant;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private Dictionary<string, object> MapToDictionary(RestaurantBase restaurant)
    {
        return new Dictionary<string, object>
        {
            { "Id", restaurant.Id.ToString() },
            { "Name", restaurant.Name },
            { "City", restaurant.City },
            { "Type", restaurant.GetType().Name }
        };
    }
}
