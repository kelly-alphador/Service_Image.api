using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime UploadDate { get; set; }

    }
}
