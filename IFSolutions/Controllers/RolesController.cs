using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IFSolutions.Models;

namespace IFSolutions.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> _userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

        // GET: Roles
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        // GET
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                db.Roles.Add(new IdentityRole() { Name = collection["RoleName"] });
                db.SaveChanges();

                ViewBag.ResultMessage = "Role created successfully!";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string RoleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Roles.Remove(thisRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET
        [HttpGet]
        public ActionResult Edit(string RoleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ManageUserRoles(string userEmail)
        {
            List<UsersRolesViewModel> list = new List<UsersRolesViewModel>();

            IEnumerable<User> users;

            if (String.IsNullOrEmpty(userEmail))
            {
                users = db.Users.OrderBy(m => m.FirstName).ToList();
            }
            else
            {
                users = db.Users.Where(m => m.Email.Equals(userEmail, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(m => m.FirstName).ToList();
            }

            foreach (User user in users)
            {
                var userItem = new UsersRolesViewModel()
                {
                    User = user,
                    RoleName = _userManager.GetRoles(user.Id).FirstOrDefault()
                };

                list.Add(userItem);
            }

            return View(list.ToList());
        }

        [HttpGet]
        public ActionResult ChangeRole(string id)
        {
            UsersRolesViewModel viewModel = new UsersRolesViewModel()
            {
                User = db.Users.Where(m => m.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault(),
                UserId = id,
                RoleName = _userManager.GetRoles(id).FirstOrDefault(),
                NewRole = _userManager.GetRoles(id).FirstOrDefault()
            };

            var roles = db.Roles.Select(m => new SelectListItem
                {
                    Value = m.Name,
                    Text = m.Name
                });

            ViewBag.Roles = roles;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(UsersRolesViewModel newUserProperties)
        {
            _userManager.AddToRole(newUserProperties.UserId, newUserProperties.NewRole);

            if (newUserProperties.RoleName != null)
                _userManager.RemoveFromRole(newUserProperties.UserId, newUserProperties.RoleName);

            List<UsersRolesViewModel> list = new List<UsersRolesViewModel>();

            var users = db.Users.OrderBy(m => m.FirstName).ToList();

            foreach (User user in users)
            {
                var userItem = new UsersRolesViewModel()
                {
                    User = user,
                    RoleName = _userManager.GetRoles(user.Id).FirstOrDefault()
                };

                list.Add(userItem);
            }

            return View("ManageUserRoles", list);
        }
    }
}