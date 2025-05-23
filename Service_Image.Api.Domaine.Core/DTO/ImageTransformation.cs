using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class ImageTransformation
    {
        public Guid Id { get; set; }
        public Guid OriginalImageId { get; set; }
        public Image OriginalImage { get; set; }
        public string TransformationParameters { get; set; } // JSON serialized
        public string TransformedUrl { get; set; }
        public DateTime TransformationDate { get; set; }
        public string StoredFileName { get; set; }
        public string ContentType { get; set; }
    }
}
