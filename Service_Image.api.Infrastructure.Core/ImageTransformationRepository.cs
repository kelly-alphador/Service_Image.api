using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Image.Api.Domaine.Core.DTO;
using Service_Image.api.Infrastructure.Core.Data;
using Service_Image.Api.Domaine.Core;

namespace Service_Image.api.Infrastructure.Core
{
    public class ImageTransformationRepository:IImageTransformationRepository
    {
        private readonly ImageDbContext _context;

        public ImageTransformationRepository(ImageDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ImageTransformation transformation)
        {
            await _context.Transforms.AddAsync(transformation);
        }
    }
}
