using karciSinav.Models;
using karciSinav.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace karciSinav.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Ogretmen) + ", " + nameof(UserRoles.Admin) + ", " + nameof(UserRoles.Ogrenci))]
    public class TakeSinavController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: TakeQuiz


        [Authorize]
        public ActionResult TakeQuiz(int id)
        {
            var quiz = _context.TQuizz.Include(r => r.Questions).FirstOrDefault(q => q.QuizId == id);
            var userId = User.Identity.GetUserId();

            if (quiz == null || userId == null)
            {
                return RedirectToAction("Index", "Ana Sayfa");
            }

            var checkIfExits = _context.TOgrenciSinav.FirstOrDefault(r => r.OgrenciId == userId && r.SinavId == Sinav.SinavId);

            if (checkIfExits != null)
            {
                return RedirectToAction("Index", "Ana Sayfa");
            }
            else
            {
                var LoadSinav = new OgrenciSinav()
                {
                    SinavId = Sinav.SinavId,
                    OgrenciId = userId,
                    StartTime = DateTime.Now

                };
                _context.TSinav.Add(LoadSinav);
                _context.SaveChanges();

                var listSinav = new List<OgrenciSinavSoru>();
                var Sorus = Sinav.Sorus;

                foreach (var q in sorus)
                {

                    var addSinav = new OgrenciSinavSoru
                    {
                        OgrenciSinavId = LoadSinav.OgrenciQuizId,
                        SoruId = q.SoruId
                    };

                    listSinav.Add(addSinav);
                    _context.TOgrenciSinavSoru.Add(addSoru);
                    _context.SaveChanges();

                }
                // random listQuiz
                Random random = new Random();
                var ListaRandom = listSinav.OrderBy(x => random.Next()).ToList();

                for (int i = 0; i < ListaRandom.Count; i++)
                {
                    try
                    {
                        ListaRandom[i].NextOgrenciSoruId = ListaRandom[i + 1].OgrenciSinavSoruId;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //expected exception
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("SinavSoru", new { id = ListaRandom.First().OgrenciSinavSoruId });
            }
        }

        public ActionResult SinavSoru(int id)
        {
            var temp = _context.TOgrenciSinavSoru.Include(r => r.Soru).FirstOrDefault(x => x.OgrenciSinavSoruId == id);
            var respuestasTemp = _context.TCevap.Where(x => x.SoruId == temp.SoruId);

            var viewModel = new SorusChoiceViewModel
            {
                OgrenciSinavSoruId = temp.OgrenciSinavSoruId,
                SoruText = temp.Soru.SoruText,
                Cevaps = respuestasTemp.ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult SinavSoru (SorusChoiceViewModel choice)
        {
            var currentRecord = _context.TOgrenciSinavSoru.
                Where(x => x.OgrenciSinavSoruId == choice.OgrenciSinavSoruId).FirstOrDefault();
            if (currentRecord.NextStudentQuestionId != null)
            {
                currentRecord.SelectedCevapId = choice.SelectedCevapId;
                _context.SaveChanges();
            }
            else
            {
                currentRecord.SelectedCevapId = choice.SelectedCevapId;
                _context.SaveChanges();

                var OgrenciSinav = _context.TOgrenciSinav.Include(q => q.OgrenciSinavSorus).
                    Include(q => q.OgrenciSinavSorus.Select(z => z.Soru)).
                    Where(s => s.OgrenciSinavId == currentRecord.OgrenciSinavId).FirstOrDefault();

                var correctCevaps = studentSinav.OgrenciSinavSorus.Where(q => q.SelectedCevapId == q.Question.CorrectCevapId).Count();
                var totalSorus = studentSinav.OgrenciSinavSorus.Count();
                var finalScore = (correctCevaps * 100) / totalSorus;
                OgrenciSinav.FinalScore = finalScore;
                OgrenciSinav.EndTime = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("SinavResult", new { id = OgrenciSinav.OgrenciSinavId });
            }
            return RedirectToAction("SinavSoru", new { id = currentRecord.NextOgrenciSoruId });
        }


        public ActionResult SinavResult(int id)
        {
            var viewModel = _context.TOgrenciSinav.Where(sq => sq.OgrenciSinavId == id).FirstOrDefault();
            return View(viewModel);
        }
    }
}