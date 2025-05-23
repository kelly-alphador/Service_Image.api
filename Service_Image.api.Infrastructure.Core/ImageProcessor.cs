using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Image.Api.Domaine.Core;
using Service_Image.Api.Domaine.Core.DTO;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace Service_Image.api.Infrastructure.Core
{
    public class ImageProcessor:IImageProcessor
    {
        public async Task<Stream> ApplyTransformationsAsync(Stream imageStream, TransformationParameters parameters)
        {
            try
            {
                // Charger l'image depuis le stream
                imageStream.Position = 0;
                using var image = await SixLabors.ImageSharp.Image.LoadAsync(imageStream);

                // Appliquer les transformations
                image.Mutate(x => ApplyTransformations(x, parameters));

                // Préparer le stream de sortie
                var outputStream = new MemoryStream();

                // Déterminer l'encodeur selon le format demandé
                IImageEncoder encoder = GetEncoder(parameters.Format);

                // Sauvegarder l'image transformée
                await image.SaveAsync(outputStream, encoder);
                outputStream.Position = 0;

                return outputStream;
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Failed to process image", ex);
            }
        }

        private void ApplyTransformations(IImageProcessingContext context, TransformationParameters parameters)
        {
            // Redimensionnement
            if (parameters.Resize != null)
            {
                context.Resize(new ResizeOptions
                {
                    Size = new Size(parameters.Resize.Width ?? 0, parameters.Resize.Height ?? 0),
                    Mode = ResizeMode.Max
                });
            }

            // Recadrage
            if (parameters.Crop != null)
            {
                context.Crop(new Rectangle(
                    parameters.Crop.X ?? 0,
                    parameters.Crop.Y ?? 0,
                    parameters.Crop.Width ?? 0,
                    parameters.Crop.Height ?? 0));
            }

            // Rotation
            if (parameters.Rotate.HasValue)
            {
                context.Rotate(parameters.Rotate.Value);
            }

            // Filtres
            if (parameters.Filters != null)
            {
                if (parameters.Filters.Grayscale)
                    context.Grayscale();

                if (parameters.Filters.Sepia)
                    context.Sepia();
            }
        }

        private IImageEncoder GetEncoder(string format)
        {
            return format?.ToLower() switch
            {
                "png" => new PngEncoder(),
                "gif" => new GifEncoder(),
                "bmp" => new BmpEncoder(),
                _ => new JpegEncoder() // Par défaut JPEG
            };
        }
    }
}
