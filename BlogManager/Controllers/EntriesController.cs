using BlogManager.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

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
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new EntriesViewModel
            {
                Entries = _context.Entries.Include(e => e.Account).Include(e => e.EntryCategory).ToList()
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Validate(int entryId, string isVisible)
        {
            var entryToValidate = _context.Entries.SingleOrDefault(e => e.Id == entryId);
            if (entryToValidate == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if(account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            entryToValidate.LastModifiedBy = account;
            entryToValidate.LastModification = DateTime.Now;

            switch (isVisible.ToLower())
            {
                case "true":
                    entryToValidate.IsVisible = true;
                    _context.SaveChanges();
                    break;
                case "false":
                    entryToValidate.IsVisible = false;
                    _context.SaveChanges();
                    break;
                default:
                    break;
            }        

            return RedirectToAction("Index", "Entries");
        }

        [HttpPost]
        public ActionResult Delete(int entryId)
        {
            var entryToDelete = _context.Entries.SingleOrDefault(e => e.Id == entryId);
            if (entryToDelete == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if (account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            _context.Entries.Remove(entryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Entries");
        }
    }
}