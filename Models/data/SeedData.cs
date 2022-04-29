using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace karciSinav.Models.data
{
    public static class SeedData
    {
        public static void Initialize()
        {
            var context = new ApplicationDbContext();

            string[] roles = new string[] { "Admin", "Ogretmen", "Ogrenci"};

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    Task.Run(() => roleStore.CreateAsync(new IdentityRole(role))).Wait();
                }
            }

            context.SaveChanges();
        }

    }
}