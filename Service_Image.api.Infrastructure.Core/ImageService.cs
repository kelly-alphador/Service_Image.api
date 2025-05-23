using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Service_Image.Api.Domaine.Core;
using Service_Image.Api.Domaine.Core.DTO;
using SixLabors.ImageSharp;
using X.PagedList;

namespace Service_Image.api.Infrastructure.Core
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorageService _storageService;
        private readonly IImageProcessor _imageProcessor;
        private readonly IImageRepository _imageRepository;
        private readonly IImageTransformationRepository _transformationRepository;

        public ImageService(
            IUnitOfWork unitOfWork,
            IImageStorageService storageService,
            IImageProcessor imageProcessor,
            IImageRepository imageRepository,
            IImageTransformationRepository transformationRepository)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _imageProcessor = imageProcessor;
            _imageRepository = imageRepository;
            _transformationRepository = transformationRepository;
        }
        //IfromFile une fichier envoyer depuis frontend
        public async Task<ImageDto> UploadImageAsync(IFormFile file,string idUser)
        {
            //pour verifier si l'image est null ou vide
            if (file == null || file.Length == 0)
                //on lance une xception
                throw new ArgumentException("No file uploaded");
            //creation de buffer vide en RAM
            using var imageStream = new MemoryStream();
            //on copie le fichier dans le buffer pour avoir le copie de l'image
            await file.CopyToAsync(imageStream);
            //place le curseur au debut
            imageStream.Position = 0;
            //on utilise le ImageSharp pour analyser l'image
            var imageInfo = await SixLabors.ImageSharp.Image.IdentifyAsync(imageStream);
            if (imageInfo == null)
                throw new ArgumentException("Invalid image file");

            imageStream.Position = 0;
            //on utilise Guid.NewGuid pour avoir une identifiant unique
            //Path.GetExtension pour avoir l'extension de l'image (.png,.jpg)
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            //upload 
            var imageUrl = await _storageService.UploadImageAsync(imageStream, fileName, file.ContentType);
            //on recupere tous les donnees pour pouvoir ajouter dans base de donnees
            var image = new Api.Domaine.Core.DTO.Image 
            {
                OriginalFileName = file.FileName,
                StoredFileName = fileName,
                ContentType = file.ContentType,
                SizeInBytes = file.Length,
                OriginalUrl = imageUrl,
                UploadDate = DateTime.UtcNow,
                Width = imageInfo.Width,
                Height = imageInfo.Height,
                UploadedByUserId= idUser
            };
            
            //prepare l'insertion de l'entite image dans EF core
            await _imageRepository.AddAsync(image);
            //ajoute a la base de donnee
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(image);
        }

        public async Task<ImageTransformationResult> TransformImageAsync(Guid id, ImageTransformationRequest request)
        {
            //
            var image = await _imageRepository.GetByIdAsync(id);
            if (image == null)
                throw new KeyNotFoundException("Image not found");
            //telecharger l'image
            await using var originalStream = await _storageService.GetImageAsync(image.StoredFileName);

            var parameters = new TransformationParameters
            {
                Resize = request.Resize,
                Crop = request.Crop,
                Rotate = request.Rotate,
                Format = request.Format,
                Filters = request.Filters
            };
            //appliquer le transformation
            await using var transformedStream = await _imageProcessor.ApplyTransformationsAsync(originalStream, parameters);

            var fileExtension = string.IsNullOrEmpty(request.Format) ? "jpg" : request.Format.ToLower();
            var transformedFileName = $"{Guid.NewGuid()}.{fileExtension}";
            var contentType = GetContentType(request.Format);
            var transformedUrl = await _storageService.UploadTransformedImageAsync(transformedStream, transformedFileName, contentType);

            var transformation = new ImageTransformation
            {
                OriginalImageId = image.Id,
                TransformationParameters = JsonSerializer.Serialize(parameters),
                TransformedUrl = transformedUrl,
                TransformationDate = DateTime.UtcNow,
                StoredFileName = transformedFileName,
                ContentType = contentType
            };

            await _transformationRepository.AddAsync(transformation);
            await _unitOfWork.SaveChangesAsync();

            return new ImageTransformationResult
            {
                Url = transformedUrl,
                Metadata = new
                {
                    TransformationId = transformation.Id,
                    OriginalImageId = image.Id,
                    CreatedAt = transformation.TransformationDate,
                    Parameters = parameters
                }
            };
        }

        public async Task<ImageDto> GetImageAsync(Guid id)
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image == null)
                throw new KeyNotFoundException("Image not found");

            return MapToDto(image);
        }

        public async Task<IPagedList<ImageDto>> GetImagesAsync(int page, int limit)
        {
            var images = await _imageRepository.GetAllAsync(page, limit);
            var dtos = images.Select(MapToDto).ToList();

            return new StaticPagedList<ImageDto>(
                dtos,
                images.PageNumber,
                images.PageSize,
                images.TotalItemCount);
        }

        private ImageDto MapToDto(Api.Domaine.Core.DTO.Image image) => new()
        {
            Id = image.Id,
            Url = image.OriginalUrl,
            FileName = image.OriginalFileName,
            Size = image.SizeInBytes,
            Width = image.Width,
            Height = image.Height,
            UploadDate = image.UploadDate
        };

        private string GetContentType(string format) => format?.ToLower() switch
        {
            "png" => "image/png",
            "bmp" => "image/bmp",
            "gif" => "image/gif",
            _ => "image/jpeg"
        };
    }
}