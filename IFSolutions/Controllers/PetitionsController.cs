﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IFSolutions.Models;
using System.Data.Entity.Validation;

namespace IFSolutions.Controllers
{
    [Authorize]
    public class PetitionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Petitions
        public ActionResult Index(string filterText)
        {
            if (!User.IsInRole("Administrator") && !User.IsInRole("Employee"))
            {
                var userId = User.Identity.GetUserId();

                if (!String.IsNullOrEmpty(filterText))
                {
                    var filteredPetitions = db.Petitions.Include(p => p.Campus).Include(p => p.Category).Include(p => p.User)
                        .Where(p => p.UserId.Equals(userId, StringComparison.CurrentCultureIgnoreCase))
                        .Where(p => p.Title.Contains(filterText)).OrderByDescending(p => p.DateCreated);

                    return View(filteredPetitions.ToList());
                }

                var petitions = db.Petitions.Include(p => p.Campus).Include(p => p.Category).Include(p => p.User)
                    .Where(p => p.UserId.Equals(userId, StringComparison.CurrentCultureIgnoreCase)).OrderByDescending(p => p.DateCreated);

                return View(petitions.ToList());
            }

            if (!String.IsNullOrEmpty(filterText))
            {
                var filteredPetitions = db.Petitions.Include(p => p.Campus).Include(p => p.Category).Include(p => p.User)
                    .Where(p => p.Title.Contains(filterText)).OrderByDescending(p => p.DateCreated);

                return View(filteredPetitions.ToList());
            }

            var allPetitions = db.Petitions.Include(p => p.Campus).Include(p => p.Category).Include(p => p.User)
                    .OrderByDescending(p => p.DateCreated);

            return View(allPetitions.ToList());
        }

        // GET: Petitions/Create
        public ActionResult Create()
        {
            var campi = db.Campus.OrderBy(m => m.Description);
            ViewBag.CampusID = new SelectList(campi, "CampusID", "Description");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description");
            return View();
        }

        // POST: Petitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePetitionViewModel petition)
        {
            Petition newPetition = new Petition()
            {
                CampusID = petition.CampusID,
                CategoryID = petition.CategoryID,
                Title = petition.Title,
                Description = petition.Description,
                DateCreated = DateTime.Now,
                Solved = false,
                UserId = User.Identity.GetUserId()
            };

            if (ModelState.IsValid)
            {
                db.Petitions.Add(newPetition);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            var campi = db.Campus.OrderBy(m => m.Description);
            ViewBag.CampusID = new SelectList(campi, "CampusID", "Description", petition.CampusID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", petition.CategoryID);
            return View(petition);
        }

        // GET: Petitions/Edit/5
        [Authorize(Roles = "Administrator, Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Petition petition = db.Petitions.Find(id);

            EditPetitionViewModel editPetition = new EditPetitionViewModel
            {
                PetitionID = petition.PetitionID,
                CategoryID = petition.CategoryID,
                CampusID = petition.CampusID,
                Title = petition.Title,
                Description = petition.Description
            };

            if (petition == null)
            {
                return RedirectToAction("Index");
            }
            
            ViewBag.CampusID = new SelectList(db.Campus, "CampusID", "Description", petition.CampusID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", petition.CategoryID);

            return View(editPetition);
        }

        // POST: Petitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPetitionViewModel petition)
        {
            Petition modifiedPetition = db.Petitions.Find(petition.PetitionID);
            modifiedPetition.CategoryID = petition.CategoryID;
            modifiedPetition.CampusID = petition.CampusID;
            modifiedPetition.Title = petition.Title;
            modifiedPetition.Description = petition.Description;

            if (ModelState.IsValid)
            {
                db.Entry(modifiedPetition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusID = new SelectList(db.Campus, "CampusID", "Description", petition.CampusID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", petition.CategoryID);

            return View(petition);
        }

        // GET: Petitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Petition petition = db.Petitions.Find(id);
            if (petition == null)
            {
                return HttpNotFound();
            }
            if (petition.UserId.Equals(User.Identity.GetUserId(), StringComparison.CurrentCultureIgnoreCase)
                        || User.IsInRole("Administrator") || User.IsInRole("Employee"))
            {
                return View(petition);
            }
            return RedirectToAction("Index");
        }

        // POST: Petitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Petition petition = db.Petitions.Find(id);
            db.Petitions.Remove(petition);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Employee, Administrator")]
        public ActionResult Complaints()
        {
            IEnumerable<Petition> petitions = db.Petitions.Where(m => m.Complaints.Count > 0)
                .OrderBy(m => m.Complaints.Count).ToList();

            return View(petitions);
        }

        [Authorize(Roles = "Employee, Administrator")]
        public ActionResult EmptyComplaints(int id)
        {
            IEnumerable<PetitionComplaint> complaints = db.PetitionComplaints.Where(m => m.PetitionID == id);

            if (complaints.Count() > 0)
            {
                foreach (PetitionComplaint complaint in complaints)
                {
                    db.PetitionComplaints.Remove(complaint);
                }

                db.SaveChanges();
            }

            IEnumerable<Petition> petitions = db.Petitions.Where(m => m.Complaints.Count > 0)
                .OrderBy(m => m.Complaints.Count).ToList();

            return RedirectToAction("Complaints");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
