﻿using BlogManager.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using BlogManager.Models.Entries;
using BlogManager.Helpers;
using BlogManager.Models.Accounts;

namespace BlogManager.Controllers
{
    public class EntriesController : Controller
    {
        private ApplicationDbContext _context;
        private Account _signedUser;

        public EntriesController()
        {
            _context = new ApplicationDbContext();
            _signedUser = new Account();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        private void GetSignedUser()
        {
            _signedUser = _context.Users
                .Include(u => u.AccountType)
                .SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new EntriesViewModel
            {
                Entries = _context.Entries
                    .Include(e => e.Account)
                    .Include(e => e.EntryCategory)
                    .ToList()
            };

            GetSignedUser();
            if (!_signedUser.CanManageAllContent())
                viewModel.Entries = viewModel.Entries.Where(e => e.Account.Equals(_signedUser)).ToList();       

            return View(viewModel);
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
            var dbEntry = _context.Entries
                .Include(e => e.EntryCategory)
                .Include(e => e.Account)
                .SingleOrDefault(e => e.Id == id);

            if (dbEntry == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbEntry.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            var viewModel = new EntryViewModel
            {
                Entry = dbEntry,
                EntryCategories = _context.EntryCategories.Where(c => c.IsActive).ToList()
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

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbEntry.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            dbEntry.Paragraphs = _context.Paragraphs.Where(p => p.EntryId == dbEntry.Id).ToList();

            var viewModel = new EntryPreviewViewModel
            {
                Entry = dbEntry
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Entry entry)
        {
            var dbEntry = _context.Entries
                .Include(e => e.EntryCategory)
                .SingleOrDefault(e => e.Id == entry.Id);

            if (dbEntry == null)
            {
                entry.NormalizeEntry();
                entry.CreateDate = DateTime.Now;
                entry.Account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                entry.Title = entry.Title;
                entry.Description = entry.Description;
                entry.Content = entry.Content;
                entry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);
                entry.IsVisible = false;

                entry.GetParagraphsFromContent();

                _context.Entries.Add(entry);

                foreach (var p in entry.Paragraphs)
                    _context.Paragraphs.Add(p);
            }
            else
            {
                GetSignedUser();
                if (!_signedUser.CanManageAllContent() && !dbEntry.Account.Equals(_signedUser))
                    return RedirectToAction("Index", "Home");

                entry.NormalizeEntry();

                if (!dbEntry.Equals(entry))
                    dbEntry.IsVisible = false;

                dbEntry.Title = entry.Title;
                dbEntry.Description = entry.Description;
                dbEntry.Content = entry.Content;
                dbEntry.ImageUrl = entry.ImageUrl;
                dbEntry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == entry.EntryCategory.Id);
                dbEntry.LastModifiedBy = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                dbEntry.LastModification = DateTime.Now;

                dbEntry.GetParagraphsFromContent();

                var dbParagraphs = _context.Paragraphs.Where(p => p.EntryId == entry.Id).ToList();
                foreach (var p in dbParagraphs)
                    _context.Paragraphs.Remove(p);
                dbParagraphs.Clear();
                dbParagraphs = dbEntry.Paragraphs;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Entries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int entryId, string isVisible)
        {
            GetSignedUser();
            if (!_signedUser.CanManageAllContent())
                return RedirectToAction("Index", "Home");

            var entryToValidate = _context.Entries.SingleOrDefault(e => e.Id == entryId);

            if (entryToValidate == null)
                return HttpNotFound();
 
            entryToValidate.LastModifiedBy = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int entryId)
        {
            var entryToDelete = _context.Entries.SingleOrDefault(e => e.Id == entryId);

            if (entryToDelete == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !entryToDelete.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            _context.Entries.Remove(entryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Entries");
        }
    }
}