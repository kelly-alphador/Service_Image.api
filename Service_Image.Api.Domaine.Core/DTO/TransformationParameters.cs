using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class TransformationParameters
    {
        public ResizeParameters Resize { get; set; }
        public CropParameters Crop { get; set; }
        public int? Rotate { get; set; }
        public string Format { get; set; }
        public FiltersParameters Filters { get; set; }
    }
}
