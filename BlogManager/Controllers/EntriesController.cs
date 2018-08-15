using BlogManager.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntriesController : Controller
    {
        private ApplicationDbContext _context;

        public EntriesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var viewModel = new EntriesViewModel
            {
                Entries = _context.Entries.Include(e => e.Account).Include(e => e.EntryCategory).ToList()
            };
            return View(viewModel);
        }
        
        public ActionResult Validate(int entryId, string isVisible)
        {
            var entryToValidate = _context.Entries.Single(e => e.Id == entryId);

            switch (isVisible)
            {
                case "true":
                    entryToValidate.IsVisible = true;
                    entryToValidate.LastModification = DateTime.Now;
                    entryToValidate.LastModifiedBy = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                    _context.SaveChanges();
                    break;
                case "false":
                    entryToValidate.IsVisible = false;
                    entryToValidate.LastModification = DateTime.Now;
                    entryToValidate.LastModifiedBy = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                    _context.SaveChanges();
                    break;
                default:
                    break;
            }        

            return RedirectToAction("Index", "Entries");
        }
    }
}