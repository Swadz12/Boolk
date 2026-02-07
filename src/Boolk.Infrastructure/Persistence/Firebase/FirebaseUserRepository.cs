using Boolk.Application.Interfaces;
using Boolk.Domain.Entities;
using Google.Cloud.Firestore;

namespace Boolk.Infrastructure.Persistence.Firebase;

/// <summary>
/// Firebase implementation of the user repository.
/// </summary>
public class FirebaseUserRepository : IUserRepository
{
    private readonly FirestoreDb _db;
    private const string CollectionName = "users";

    public FirebaseUserRepository(FirestoreDb db)
    {
        _db = db;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        var snapshot = await docRef.GetSnapshotAsync();
        
        if (!snapshot.Exists) return null;
        
        return MapToUser(snapshot);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var snapshot = await _db.Collection(CollectionName)
            .WhereEqualTo("Email", email)
            .Limit(1)
            .GetSnapshotAsync();
        
        if (snapshot.Documents.Count == 0) return null;
        
        return MapToUser(snapshot.Documents[0]);
    }

    public async Task<User> CreateAsync(User user)
    {
        if (user.Id == Guid.Empty)
            user.Id = Guid.NewGuid();

        var docRef = _db.Collection(CollectionName).Document(user.Id.ToString());
        await docRef.SetAsync(MapToDictionary(user));
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var docRef = _db.Collection(CollectionName).Document(user.Id.ToString());
        await docRef.SetAsync(MapToDictionary(user), SetOptions.MergeAll);
    }

    public async Task DeleteAsync(Guid id)
    {
        var docRef = _db.Collection(CollectionName).Document(id.ToString());
        await docRef.DeleteAsync();
    }

    private User? MapToUser(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        if (data == null) return null;
        
        var birthDate = DateTime.MinValue;
        if (data.ContainsKey("BirthDate") && data["BirthDate"] is Timestamp ts)
        {
            birthDate = ts.ToDateTime();
        }
        
        return new User
        {
            Id = Guid.Parse(doc.Id),
            Email = data["Email"].ToString() ?? "",
            PasswordHash = data["PasswordHash"].ToString() ?? "",
            Name = data["Name"].ToString() ?? "",
            BirthDate = birthDate
        };
    }

    private Dictionary<string, object> MapToDictionary(User user)
    {
        return new Dictionary<string, object>
        {
            { "Id", user.Id.ToString() },
            { "Email", user.Email },
            { "PasswordHash", user.PasswordHash },
            { "Name", user.Name },
            { "BirthDate", DateTime.SpecifyKind(user.BirthDate, DateTimeKind.Utc) },
        };
    }
}
