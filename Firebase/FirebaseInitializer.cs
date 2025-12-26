using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.IO;

namespace Boolk.Firebase;

public static class FirebaseInitializer
{
    private static FirestoreDb? _firestoreDb;

    public static void Initialize(FirebaseConfig config)
    {
        var credentialsPath = Path.IsPathRooted(config.CredentialsPath) 
            ? config.CredentialsPath 
            : Path.Combine(Directory.GetCurrentDirectory(), config.CredentialsPath);

        if (!File.Exists(credentialsPath))
        {
            throw new FileNotFoundException(
                $"Firebase credentials file not found at: {credentialsPath}. " +
                $"Please ensure the file exists and the path is correct in appsettings.json");
        }

        GoogleCredential credential = GoogleCredential.FromFile(credentialsPath);
        
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
                ProjectId = config.ProjectId
            });
        }

        _firestoreDb = new FirestoreDbBuilder
        {
            ProjectId = config.ProjectId,
            Credential = credential
        }.Build();
    }

    public static FirestoreDb GetFirestoreDb()
    {
        if (_firestoreDb == null)
            throw new InvalidOperationException("Firebase not initialized. Call Initialize first.");
        
        return _firestoreDb;
    }
}