using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service_Image.Api.Domaine.Core;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace Service_Image.api.Infrastructure.Core
{
    public class LocalImageStorageService:IImageStorageService
    {
        private readonly string _storagePath;
        private readonly string _baseUrl;

        public LocalImageStorageService(IConfiguration configuration)
        {
            _storagePath = configuration["ImageStorage:LocalPath"] ?? "wwwroot/images";
            _baseUrl = configuration["ImageStorage:BaseUrl"] ?? "/images";

            // Créer le répertoire s'il n'existe pas
            Directory.CreateDirectory(_storagePath);
            Directory.CreateDirectory(Path.Combine(_storagePath, "transformed"));
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            await SaveFileAsync(imageStream, filePath);
            return $"{_baseUrl}/{fileName}";
        }

        public async Task<Stream> GetImageAsync(string filePath)
        {
            var fullPath = Path.Combine(_storagePath, filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Image not found", fullPath);

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(fullPath, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<string> UploadTransformedImageAsync(Stream imageStream, string fileName, string contentType)
        {
            var filePath = Path.Combine(_storagePath, "transformed", fileName);
            await SaveFileAsync(imageStream, filePath);
            return $"{_baseUrl}/transformed/{fileName}";
        }

        public Task DeleteImageAsync(string filePath)
        {
            var fullPath = Path.Combine(_storagePath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        private async Task SaveFileAsync(Stream stream, string filePath)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            stream.Position = 0;
            await stream.CopyToAsync(fileStream);
        }
    }
}
