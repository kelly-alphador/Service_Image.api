﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class CropParameters
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }

    }
}
