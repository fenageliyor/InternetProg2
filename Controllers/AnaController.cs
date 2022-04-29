using Examen.Models;
using Examen.ViewModel;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using System;
using System.Text;
using System.Collections.Generic;
using karciSinav.Utility;

namespace karciSinav.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Ogretmen) + ", " + nameof(UserRoles.Admin) + ", " + nameof(UserRoles.Ogrenci))]
    public class AnaController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            //TODO Navigate to edit test page
            if (User.IsInRole(UserRoles.Ogretmen.ToString()))
            {
                return RedirectToAction("Index", "Dersler");
            }
      
            return View();
        }


        [HttpPost]
        public ActionResult Index(OgrenciDersViewModel info)
        {

            if (ModelState.IsValid)
            {
                var Soru = _context.TSoru.FirstOrDefault(q => q.SoruCode == info.SoruCode && q.Active);

                if (Soru== null)
                {
                    return RedirectToAction("Index", "Ana Sayfa");
                }

                var studentCourse = new OgrenciDers
                {
                    OgrenciId = User.Identity.GetUserId(),
                    Ders = info.Ders,
                    DersId = Sinav.DersId
                };

                //todo:only add if doesnt exist check first
                var checkIfExist = _context.TOgrenciDers.Any(q => q.OgrenciId == OgrenciDers.OgrenciId && q.DersId == OgrenciDers.DersId);

                if (!checkIfExist)
                {
                    _context.TOgrenciDers.Add(ogrenciDers);
                    _context.SaveChanges();
                }

                return RedirectToAction("TakeSinav", "TakeSinav",new { id=Sinav.SinavId});
            }
            else
            {
                return RedirectToAction("Index", "Ana Sayfa");
            }

           
        }

        

    }
}