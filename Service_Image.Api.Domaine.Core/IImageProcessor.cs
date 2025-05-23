using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Image.Api.Domaine.Core.DTO;

namespace Service_Image.Api.Domaine.Core
{
    public interface IImageProcessor
    {
        Task<Stream> ApplyTransformationsAsync(Stream imageStream, TransformationParameters parameters);
    }
}
