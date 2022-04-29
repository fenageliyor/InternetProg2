using karciSinav.Models;
using karciSinav.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace karciSinav.Controllers
{
    //access role
    [Authorize(Roles = nameof(UserRoles.Ogretmen) + ", " + nameof(UserRoles.Admin))]
    public class SinavController : Controller
    {
        // GET: Quiz
        private ApplicationDbContext _context=new ApplicationDbContext();
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            //todo: course that belong to the user list
            var viewModel = new SinavBilgiViewModel
            {
                Courses = _context.TCourse.Where(c=>c.UserId==userId).ToList()

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(SinavBilgiViewModel Bilgi)
        {
            var SinavCreation = new Sinav
            {
                SinavName = Bilgi.SinavName,
                Active=true,
                SinavCode = CodeGenerator()

            };
            _context.TSinav.Add(SinavCreation);
            _context.SaveChanges();
            return RedirectToAction("CreateSoru", new {id= SinavCreation.SinavId} );
        }


        public ActionResult CreateSoru(int Id)
        {
            var SinavBilgi = _context.TSinav.Include(q =>q.Sorus.Select(a=>a.Cevap)).Where(q => q.SinavId == Id).FirstOrDefault();
     
            var viewModel = new SinavBilgiViewModel
            {
                SinavId=SinavBilgi.SinavId,
                SinavName=SinavBilgi.SinavName,
                SinavCode=SinavBilgi.SinavCode,
                Sorus = SinavBilgi.Sorus.Select(q => new AddSoruViewModel
                {
                    SoruText = q.SoruText,
                    Cevaps = q.Cevaps.Select(a => new CevapViewModel { CevapText = a.CevapText }).ToList()
                }).ToList()
                
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateSoru( SinavBilgiViewModel Bilgi, string[] DynamicTextBox)
        {
            var addSoru = new Soru
            {
                SoruText = Bilgi.SoruText,
                SinavId = Bilgi.SinavId,
                Cevaps = new List<Cevap>()
            };

            foreach (var item in DynamicTextBox)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    addSoru.Cevaplar.Add(
                    new Cevap
                    {
                      
                        CevapText = item
                    });
                }
            }
            _context.TSoru.Add(addSoru);
            _context.SaveChanges();

            addSoru.CorrectCevapId = addSoru.Cevaps.First().CevapId;
            _context.SaveChanges();
            return RedirectToAction ("CreateSoru", new { id = Bilgi.SinavId });
        }

        public ActionResult DeleteSinav(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var infoSinav = _context.TSinav.Find(id);
            if (infoSinav == null)
            {
                return HttpNotFound();
            }

            var q = new SinavListViewModel
            {
                SinavId= infoSinav.SinavId,
                SinavName= infoSinav.SinavName,
                SinavCode= infoSinav.SinavCode,
                Active=infoSinav.Active
              
            };        
            return View(q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSinav(SinavListViewModel info)
        {
            var Sinav = _context.TSinav.Find(info.SinavId);
            Sinav.Active = false;
            _context.SaveChanges();
            return RedirectToAction("ListSinavs");
        }


        public string CodeGenerator()
        {
            var rand = new Random();
            var code = new List<char>();
            for (var i = 0; i < 5; i++)
            {
                var asciiCode = rand.Next(97, 122);
                code.Add((char)asciiCode);
            }
            for (var i = 0; i < 3; i++)
            {
                var asciiCode = rand.Next(48, 57);
                code.Add((char)asciiCode);
            }
            code = code.OrderBy(x => rand.Next()).ToList();
            return String.Join("", code);
        }
    }
}