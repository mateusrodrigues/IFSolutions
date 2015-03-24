using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFSolutions.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Explore");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Um pouco sobre o IFSolutions.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Como nos contactar.";

            return View();
        }
    }
}