using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntryController : Controller
    {
        private ApplicationDbContext _context;

        public EntryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult New()
        {
            var viewModel = new EntryViewModel
            {
                Entry = new Entry(),
                EntryCategories = _context.EntryCategories.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Entry entry)
        {
            entry.CreateDate = DateTime.Now;
            entry.Account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            entry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);
            entry.IsVisible = false;

            _context.Entries.Add(entry);
            _context.SaveChanges();

            return RedirectToAction("Index", "Entries");
        }
    }
}