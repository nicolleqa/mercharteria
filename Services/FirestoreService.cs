using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;

public class FirestoreService
{
    private readonly FirestoreDb _firestoreDb;

    public FirestoreService(IConfiguration configuration)
    {
        var projectId = configuration["FIREBASE_PROJECT_ID"] ?? Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");

        // Construir el JSON de credenciales desde variables de entorno (ideal para producción)
        var credentialJson = $@"
        {{
            ""type"": ""service_account"",
            ""project_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")}"",
            ""private_key_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID")}"",
            ""private_key"": ""{Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY")?.Replace("\\n", "\n")}"",
            ""client_email"": ""{Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL")}"",
            ""client_id"": ""{Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID")}"",
            ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
            ""token_uri"": ""https://oauth2.googleapis.com/token"",
            ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
            ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/{Uri.EscapeDataString(Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL") ?? "")}""
        }}";

        GoogleCredential credential;

        // Si existe el archivo de credenciales, úsalo (ideal para desarrollo)
        var credentialPath = configuration["Firebase:CredentialPath"];
        if (!string.IsNullOrEmpty(credentialPath) && System.IO.File.Exists(credentialPath))
        {
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }
        }
        else
        {
            // Si no, usa las variables de entorno (ideal para producción)
            credential = GoogleCredential.FromJson(credentialJson);
        }

        // Crear el FirestoreClient usando el GoogleCredential
        var firestoreClient = new FirestoreClientBuilder
        {
            Credential = credential
        }.Build();

        _firestoreDb = FirestoreDb.Create(projectId, firestoreClient);
    }

    // Método para crear un documento
    public async Task CreateDocument<T>(string collectionName, string documentId, T data)
    {
        DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
        await docRef.SetAsync(data);
    }

    // Método para obtener un documento
    public async Task<T> GetDocument<T>(string collectionName, string documentId)
    {
        DocumentReference docRef = _firestoreDb.Collection(collectionName).Document(documentId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        return snapshot.ConvertTo<T>();
    }
}