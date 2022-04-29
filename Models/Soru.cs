using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace karciSinav.Models
{
    public class Soru
    {
        public int SoruId { get; set; }
        [Required]
        public string SoruYazi { get; set; }
        [Required]
        public List<Cevap> Cevaps { get; set; }
        public int? DogruCevapId { get; set; }
        //public Cevap DogruCevap { get; set; }
        public int SinavId { get; set; }
        public Sinav Sinav { get; set; }
    }
}