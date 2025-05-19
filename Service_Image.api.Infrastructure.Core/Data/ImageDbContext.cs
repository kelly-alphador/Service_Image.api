using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Service_Image.api.Infrastructure.Core.Data
{
    //on utilise IdentityDbContext pour gerer les utilisateurs 
    //il est la responsable de creation des tables comme user ,role etc
    public class ImageDbContext: IdentityDbContext
    {
        //base (options) pour passer options au constructeur a la classe parente(IdentityDbContext)
        //sans cela le context ne sauraient pas comment connecter aux BDD
        public ImageDbContext(DbContextOptions<ImageDbContext> options):base(options)
        { }
    }
}
