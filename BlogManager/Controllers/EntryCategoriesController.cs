using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntryCategoriesController : Controller
    {
        private ApplicationDbContext _context;

        public EntryCategoriesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var viewModel = new EntryCategoriesViewModel
            {
                EntryCategories = _context.EntryCategories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Activate(int catId, string isActive)
        {
            var categoryToActivate = _context.EntryCategories.SingleOrDefault(c => c.Id == catId);
            if (categoryToActivate == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if(account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            switch (isActive.ToLower())
            {
                case "true":
                    categoryToActivate.IsActive = true;
                    _context.SaveChanges();
                    break;
                case "false":
                    categoryToActivate.IsActive = false;
                    _context.SaveChanges();
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "EntryCategories");
        }

        [HttpPost]
        public ActionResult Delete(int catId)
        {
            var categoryToDelete = _context.EntryCategories.SingleOrDefault(e => e.Id == catId);
            if (categoryToDelete == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if (account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            _context.EntryCategories.Remove(categoryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "EntryCategories");
        }
    }
}