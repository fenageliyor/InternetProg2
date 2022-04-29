using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace karciSinav.Models
{
    public class Cevap
    {
        public int CevapId { get; set; }
        [Required]
        public string CevapText { get; set; }
        [Required]
        public int SoruId { get; set; }
        public Soru Soru { get; set; }
    }
}