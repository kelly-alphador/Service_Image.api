using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Service_Image.api.Infrastructure.Core.Data;


namespace Service_Image.api.Infrastructure.Core
{
    public class ImageContextFactory : IDesignTimeDbContextFactory<ImageDbContext>
    {
        public ImageDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ImageDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-1PCHEEU\\SQLEXPRESS;Database=Traitement_Image1;Trusted_Connection=True;TrustServerCertificate=True");
            return new ImageDbContext(optionsBuilder.Options);
        }
    }
}
