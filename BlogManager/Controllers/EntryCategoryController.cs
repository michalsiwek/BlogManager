using BlogManager.Models;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntryCategoryController : Controller
    {
        private ApplicationDbContext _context;

        public EntryCategoryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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
    }
}