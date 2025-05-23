using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
