using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using BlogManager.Helpers;

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
                EntryCategories = _context.EntryCategories.Where(c => c.IsActive == true).ToList()
            };

            return View(viewModel);
        }
        
        public ActionResult Edit(int id)
        {
            var dbEntry = _context.Entries.SingleOrDefault(e => e.Id == id);

            if (dbEntry == null)
                return HttpNotFound();

            var viewModel = new EntryViewModel
            {
                Entry = dbEntry,
                EntryCategories = _context.EntryCategories.Where(c => c.IsActive == true).ToList()
            };

            return View(viewModel);
        }

        public ActionResult Preview(Entry entry)
        {
            var dbEntry = _context.Entries
                .Include(e => e.Account)
                .Include(e => e.EntryCategory)
                .SingleOrDefault(e => e.Id == entry.Id);

            if (dbEntry == null)
                return HttpNotFound();

            var viewModel = new EntryPreviewViewModel
            {
                Entry = dbEntry
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save(Entry entry)
        {
            var dbEntry = _context.Entries.SingleOrDefault(e => e.Id == entry.Id);
            var dbParagraphs = _context.Paragraphs.Where(p => p.Entry_Id == entry.Id).ToList();

            if (dbEntry == null)
            {
                entry.CreateDate = DateTime.Now;
                entry.Account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                entry.Content = entry.Content;
                entry.Paragraphs = entry.GetParagraphsFromContent();
                dbParagraphs = entry.Paragraphs;
                entry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);
                entry.IsVisible = false;
                _context.Entries.Add(entry);
            }
            else
            {
                dbEntry.Title = entry.Title;
                dbEntry.Description = entry.Description;
                dbEntry.Content = entry.Content;
                dbEntry.Paragraphs = entry.GetParagraphsFromContent();
                dbParagraphs = dbEntry.Paragraphs;
                dbEntry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);
                dbEntry.IsVisible = false;
                dbEntry.LastModifiedBy = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                dbEntry.LastModification = DateTime.Now;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Entries");
        }
    }
}