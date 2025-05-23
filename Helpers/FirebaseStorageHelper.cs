using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;


namespace mercharteria.Helpers
{
    public class FirebaseStorageHelper
    {
        private readonly string? _bucketName;
        private readonly string? _rutaCredencialJson;

        public FirebaseStorageHelper(IConfiguration configuration)
        {
            _bucketName = configuration["Firebase:BucketName"];
            _rutaCredencialJson = Path.Combine(Directory.GetCurrentDirectory(), configuration["Firebase:CredentialPath"]);
        }

        public async Task<string> SubirImagenAutenticadoAsync(IFormFile file)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(_rutaCredencialJson, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

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
            GoogleCredential credential;
            using (var stream = new FileStream(_rutaCredencialJson, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

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