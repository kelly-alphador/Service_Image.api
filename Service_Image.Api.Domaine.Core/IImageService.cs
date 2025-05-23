using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Service_Image.Api.Domaine.Core.DTO;
using X.PagedList;

namespace Service_Image.Api.Domaine.Core
{
    public interface IImageService
    {
        Task<ImageDto> UploadImageAsync(IFormFile file,string idUser);
        Task<ImageTransformationResult> TransformImageAsync(Guid id, ImageTransformationRequest request);
        Task<ImageDto> GetImageAsync(Guid id);
       // Task<PagedList<ImageDto>> GetImagesAsync(int page, int limit);
        Task<IPagedList<ImageDto>> GetImagesAsync(int page, int limit);
    }
}
