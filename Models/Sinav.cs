using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace karciSinav.Models
{
    public class Sinav
    {
        public int SinavId { get; set; }
        public string SinavName { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Soru> Sorus { get; set; }
        public string SinavCode { get; set; }
        [MaxLength(125)]
        public string OgrenciId { get; set; }
        public bool Active { get; set; }
    }
}