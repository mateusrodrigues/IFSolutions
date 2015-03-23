using IFSolutions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFSolutions.Controllers
{
    [Authorize]
    public class ExploreController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Explore
        [AllowAnonymous]
        public ActionResult Index(int? campusID, bool solved = false)
        {
            ViewBag.CampiList = new SelectList(db.Campus.OrderBy(m => m.Description), "CampusID", "Description");
            ViewBag.Solved = solved;

            if (campusID.HasValue)
            {
                var listPetitions = db.Petitions.Where(m => m.CampusID == campusID && m.Solved == solved)
                    .OrderByDescending(m => m.Signatures.Count).Take(20);

                return View(listPetitions.ToList());
            }

            var listAllPetitions = db.Petitions.OrderByDescending(m => m.Signatures.Count).Where(m => m.Solved == solved).Take(20);

            return View(listAllPetitions.ToList());
        }

        [AllowAnonymous]
        public ActionResult Solved()
        {
            return View();
        }

        // GET: Explore/Details/5
        [AllowAnonymous]
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

            string userId = User.Identity.GetUserId();

            var checkNumSignature = db.Signatures
                .Where(m => m.UserId.Equals(userId, StringComparison.CurrentCultureIgnoreCase)
                && m.PetitionID == id).Count();
            
            if (checkNumSignature == 0)
            {
                ViewBag.UserSigned = false;
            }
            else
            {
                ViewBag.UserSigned = true;
            }

            return View(petition);
        }

        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult _ListComments(int petitionID)
        {
            IEnumerable<Comment> comments = db.Petitions.Where(m => m.PetitionID == petitionID).FirstOrDefault()
                .Comments.OrderBy(m => m.DateTime);
            ViewBag.PetitionID = petitionID;

            return PartialView("_ListComments", comments);
        }

        [HttpGet]
        public PartialViewResult _CommentForm(int petitionID)
        {
            return PartialView("_CommentForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _CommentForm(Comment comment)
        {
            comment.DateTime = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();

            db.Comments.Add(comment);
            db.SaveChanges();

            IEnumerable<Comment> comments = db.Petitions.Where(m => m.PetitionID == comment.PetitionID).FirstOrDefault()
                .Comments.OrderBy(m => m.DateTime);
            ViewBag.PetitionID = comment.PetitionID;

            return PartialView("_ListComments", comments);
        }

        public ActionResult SignPetition(int petitionID)
        {
            Signature signature = new Signature()
            {
                PetitionID = petitionID,
                UserId = User.Identity.GetUserId()
            };

            db.Signatures.Add(signature);
            db.SaveChanges();

            return RedirectToAction("Details/" + petitionID, "Explore");
        }

        public ActionResult UnsignPetition(int petitionID)
        {
            string userID = User.Identity.GetUserId();

            var signature = db.Signatures
                .Where(m => m.PetitionID == petitionID && m.UserId.Equals(userID, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (signature == null)
            {
                return RedirectToAction("Index");
            }

            db.Signatures.Remove(signature);
            db.SaveChanges();

            return RedirectToAction("Details/" + petitionID, "Explore");
        }

        public PartialViewResult _DeleteComment(int commentID, int petitionID)
        {
            string userID = User.Identity.GetUserId();

            var comment = db.Comments.Find(commentID);

            if (comment.UserId.Equals(userID, StringComparison.CurrentCultureIgnoreCase))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }

            IEnumerable<Comment> comments = db.Petitions.Where(m => m.PetitionID == petitionID).FirstOrDefault()
                .Comments.OrderBy(m => m.DateTime);
            ViewBag.PetitionID = petitionID;

            return PartialView("_ListComments", comments);
        }

        [Authorize(Roles = "Administrator, Employee")]
        public ViewResult MarkAsSolved(int petitionID)
        {
            Petition petition = db.Petitions.Where(m => m.PetitionID == petitionID).FirstOrDefault();
            petition.Solved = true;
            petition.DateSolved = DateTime.Now;
            db.SaveChanges();

            return View("Details", petition);
        }
    }
}