using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class Image
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public string StoredFileName { get; set; }
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadedByUserId { get; set; } // Lier à l'utilisateur si authentification implémentée
        public int Width { get; set; }
        public int Height { get; set; }

        // Pour suivre les transformations
        public ICollection<ImageTransformation> Transformations { get; set; }
    }
}
