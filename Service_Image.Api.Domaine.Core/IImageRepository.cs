using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Image.Api.Domaine.Core.DTO;
using X.PagedList;

namespace Service_Image.Api.Domaine.Core
{
    public interface IImageRepository
    {
        Task<Image> GetByIdAsync(Guid id);
        Task AddAsync(Image image);
        Task DeleteAsync(Guid id);
        Task<IPagedList<Image>> GetAllAsync(int page, int limit);
    }
}
