using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
        Task<Stream> GetImageAsync(string filePath);
        Task<string> UploadTransformedImageAsync(Stream imageStream, string fileName, string contentType);
        Task DeleteImageAsync(string filePath);
    }
}
