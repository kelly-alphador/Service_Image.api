using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Service_Image.Api.Domaine.Core.DTO;

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
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageTransformation> Transforms { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalFileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StoredFileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OriginalUrl)
                    .IsRequired();

                entity.Property(e => e.UploadDate)
                    .IsRequired();

                // Relation avec les transformations
                entity.HasMany(e => e.Transformations)
                      .WithOne(e => e.OriginalImage)
                      .HasForeignKey(e => e.OriginalImageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration de l'entité ImageTransformation
            builder.Entity<ImageTransformation>(entity =>
            {
                entity.HasKey(e => e.Id);

                // TransformationParameters est déjà une string, on configure juste le type de colonne
                entity.Property(e => e.TransformationParameters)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.TransformedUrl)
                    .IsRequired();

                entity.Property(e => e.TransformationDate)
                    .IsRequired();

                entity.Property(e => e.StoredFileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(100);

                // Relation avec l'image originale
                entity.HasOne(e => e.OriginalImage)
                      .WithMany(e => e.Transformations)
                      .HasForeignKey(e => e.OriginalImageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration pour SQL Server (optionnelle)
            if (Database.IsSqlServer())
            {
                builder.Entity<Image>()
                    .Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

                builder.Entity<ImageTransformation>()
                    .Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");
            }
        }
    }
}
