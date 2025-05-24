using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace mercharteria.Helpers
{
    public class FirebaseStorageHelper
    {
        private readonly string? _bucketName;
        private readonly string? _rutaCredencialJson;
        private readonly IConfiguration _configuration;

        public FirebaseStorageHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucketName = configuration["Firebase:BucketName"];
        }

        private GoogleCredential ObtenerCredencial()
        {
            // Construir el JSON de credenciales desde variables de entorno (ideal para producción)
            var credentialJson = $@"
            {{
                ""type"": ""service_account"",
                ""project_id"": ""{Environment.GetEnvironmentVariable("RENDER_FIREBASE_PROJECT_ID")}"",
                ""private_key_id"": ""{Environment.GetEnvironmentVariable("RENDER_FIREBASE_PRIVATE_KEY_ID")}"",
                ""private_key"": ""{Environment.GetEnvironmentVariable("RENDER_FIREBASE_PRIVATE_KEY")?.Replace("\\n", "\n")}"",
                ""client_email"": ""{Environment.GetEnvironmentVariable("RENDER_FIREBASE_CLIENT_EMAIL")}"",
                ""client_id"": ""{Environment.GetEnvironmentVariable("RENDER_FIREBASE_CLIENT_ID")}"",
                ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                ""token_uri"": ""https://oauth2.googleapis.com/token"",
                ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/{Uri.EscapeDataString(Environment.GetEnvironmentVariable("RENDER_FIREBASE_CLIENT_EMAIL") ?? "")}""
            }}";

            
                return GoogleCredential.FromJson(credentialJson);
            
        }

        public async Task<string> SubirImagenAutenticadoAsync(IFormFile file)
        {
            var credential = ObtenerCredencial();
            var storage = StorageClient.Create(credential);
            var nombreArchivo = $"PRODUCTOS/{Guid.NewGuid()}_{file.FileName}";

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await storage.UploadObjectAsync(_bucketName, nombreArchivo, file.ContentType, memoryStream);
            }

            return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(nombreArchivo)}?alt=media";
        }

        public async Task EliminarImagenAsync(string imageUrl)
        {
            var credential = ObtenerCredencial();
            var storage = StorageClient.Create(credential);

            var baseUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/";
            if (!imageUrl.StartsWith(baseUrl))
                return;

            var nombreObjeto = imageUrl.Replace(baseUrl, "").Split("?")[0];
            nombreObjeto = Uri.UnescapeDataString(nombreObjeto);

            await storage.DeleteObjectAsync(_bucketName, nombreObjeto);
        }
    }
}