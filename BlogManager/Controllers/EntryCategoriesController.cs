using BlogManager.Models;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    [Authorize(Roles = AccountTypeName.Admin + "," + AccountTypeName.Editor)]
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

        public ActionResult New()
        {
            EntryCategoryViewModel viewModel = new EntryCategoryViewModel
            {
                EntryCategory = new EntryCategory()
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var dbEntryCat = _context.EntryCategories.SingleOrDefault(c => c.Id == id);

            if (dbEntryCat == null)
                return HttpNotFound();

            if (dbEntryCat.Id == 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Forbidden action");

            var viewModel = new EntryCategoryViewModel
            {
                EntryCategory = dbEntryCat
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EntryCategory entryCategory)
        {
            var dbEntryCat = _context.EntryCategories.SingleOrDefault(c => c.Id == entryCategory.Id);

            if (dbEntryCat == null)
            {
                entryCategory.CreateDate = DateTime.Now;
                _context.EntryCategories.Add(entryCategory);
            }
            else
            {
                dbEntryCat.LastModification = DateTime.Now;
                dbEntryCat.Name = entryCategory.Name;
                dbEntryCat.Description = entryCategory.Description;
                dbEntryCat.IsActive = entryCategory.IsActive;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "EntryCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int catId, string isActive)
        {
            var categoryToActivate = _context.EntryCategories.SingleOrDefault(c => c.Id == catId);
            if (categoryToActivate == null)
                return HttpNotFound();

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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int catId)
        {
            var categoryToDelete = _context.EntryCategories.SingleOrDefault(e => e.Id == catId);
            if (categoryToDelete == null)
                return HttpNotFound();

            _context.EntryCategories.Remove(categoryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "EntryCategories");
        }
    }
}