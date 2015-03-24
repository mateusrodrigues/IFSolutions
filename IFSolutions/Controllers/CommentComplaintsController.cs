using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFSolutions.Models;

namespace IFSolutions.Controllers
{
    [Authorize(Roles = "Administrator, Employee")]
    public class CommentComplaintsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentComplaints
        public ActionResult Index()
        {
            IEnumerable<Comment> comments = db.Comments.Where(m => m.Complaints.Count > 0).OrderBy(m => m.Complaints.Count);

            return View(comments.ToList());
        }

        public ActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EmptyComplaints(int id)
        {
            IEnumerable<CommentComplaint> complaints = db.CommentComplaints.Where(m => m.CommentID == id);

            if (complaints.Count() > 0)
            {
                foreach (CommentComplaint complaint in complaints)
                {
                    db.CommentComplaints.Remove(complaint);
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}