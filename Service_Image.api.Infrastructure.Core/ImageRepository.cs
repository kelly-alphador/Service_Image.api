using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Service_Image.api.Infrastructure.Core.Data;
using Service_Image.Api.Domaine.Core;
using Service_Image.Api.Domaine.Core.DTO;
using X.PagedList;


namespace Service_Image.api.Infrastructure.Core
{
    public class ImageRepository:IImageRepository
    {
        private readonly ImageDbContext _dbContext;

        public ImageRepository(ImageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Image> GetByIdAsync(Guid id)
        {
            //recherche de l'image par id
            return await _dbContext.Images.FindAsync(id);
        }

        public async Task AddAsync(Image image)
        {
            //prepare l'ajout de l'image 
            await _dbContext.Images.AddAsync(image);
        }
       
        public async Task<IPagedList<Image>> GetAllAsync(int page, int limit)
        {
            //pour avoir le nombre de l'image
            var totalCount = await _dbContext.Images.CountAsync();
            var items = await _dbContext.Images
                .OrderByDescending(i => i.UploadDate)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new StaticPagedList<Image>(items, page, limit, totalCount);
        }


        public async Task DeleteAsync(Guid id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image != null)
            {
                _dbContext.Images.Remove(image);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
