using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Service_Image.api.Infrastructure.Core.Data;
using Service_Image.Api.Domaine.Core;

namespace Service_Image.api.Infrastructure.Core
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ImageDbContext _dbContext;
        public UnitOfWork(ImageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
