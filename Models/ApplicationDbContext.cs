using Examen.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace karciSinav.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Soru> TSoru { get; set; }
        public DbSet<Cevap> TCevap { get; set; }
        public DbSet<Sinav> TSinav { get; set; }
        public DbSet<OgrenciSinav> TOgrenciSinav { get; set; }
        public DbSet<OgrenciSinavSoru> TOgrenciSinavSoru { get; set; }
       
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        }
}