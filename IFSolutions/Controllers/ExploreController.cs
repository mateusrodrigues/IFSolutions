using IFSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFSolutions.Controllers
{
    public class ExploreController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Explore
        public ActionResult Index(int? campusID)
        {
            ViewBag.CampiList = new SelectList(db.Campus.OrderBy(m => m.Description), "CampusID", "Description");
            
            if (campusID.HasValue)
            {
                var listPetitions = db.Petitions.Where(m => m.CampusID == campusID).Where(m => m.Solved == false)
                    .OrderByDescending(m => m.Signatures.Count).Take(20);

                return View(listPetitions.ToList());
            }

            var listAllPetitions = db.Petitions.OrderByDescending(m => m.Signatures.Count).Where(m => m.Solved == false).Take(20);

            return View(listAllPetitions.ToList());
        }

        // GET: Explore/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Petition petition = db.Petitions.Find(id);

            if (petition == null)
            {
                return RedirectToAction("Index");
            }

            return View(petition);
        }
    }
}