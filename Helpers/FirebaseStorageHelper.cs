using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace mercharteria.Helpers
{
    public class FirebaseStorageHelper
    {
        private readonly string _bucketName = "merchanteria-f305c.firebasestorage.app"; //Reeemplzar por el nombre de tu bucket
        private readonly string _rutaCredencialJson = "C:\\Users\\ABRAHAM\\Documents\\SEXTO CICLO USMP\\PROGRAMACIÓN I\\merchanteria-f305c-firebase-adminsdk-fbsvc-70e602fe1a.json"; // P.ej: "C:\\Credenciales\\firebase-key.json"

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
    }
}