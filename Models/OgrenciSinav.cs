using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace karciSinav.Models
{
    public class OgrenciSinav
    {
        public int OgrenciSinavId { get; set; }
        public string OgrenciId { get; set; }
        public int SinavId { get; set; }
        public List<OgrenciSinavSoru> OgrenciSinavSorus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Decimal? FinalScore { get; set; }
    }
}