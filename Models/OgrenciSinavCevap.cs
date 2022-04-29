using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace karciSinav.Models
{
    public class OgrenciSinavSoru
    {
        public int OgrenciSinavSoruId { get; set; }
        public int? OgrenciSinavId { get; set; }
        public OgrenciSinav OgrenciSinav { get; set; }
        public int? SoruId { get; set; }
        public Soru Soru { get; set; }
        public int? SecilenCevapId { get; set; }
        public int? NextOgrenciSoruId { get; set; }
    }
}