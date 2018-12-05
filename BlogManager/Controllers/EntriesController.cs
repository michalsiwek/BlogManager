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
using BlogManager.Repositories;
using BlogManager.Infrastructure;
using BlogManager.Helpers.Enums;

namespace BlogManager.Controllers
{
    public class EntriesController : Controller
    {
        private Account _signedUser;
        private IAccountRepository _accountRepo;
        private IEntryRepository _entryRepo;
        private IEntryCategoryRepository _entryCategoryRepo;
        private IParagraphRepository _paragraphRepo;


        public EntriesController()
        {
            _signedUser = new Account();
            _accountRepo = new AccountRepository(new AccountManageService(), new MailingService());
            _entryRepo = new EntryRepository();
            _entryCategoryRepo = new EntryCategoryRepository();
            _paragraphRepo = new ParagraphRepository();
        }

        private void GetSignedUser()
        {
            _signedUser = _accountRepo.GetSignedUser(User.Identity.Name);
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new EntriesViewModel
            {
                Entries = _entryRepo.GetEntries()
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
                EntryCategories = _entryCategoryRepo.GetActiveEntryCategories()
            };

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var dbEntry = _entryRepo.GetEntry(id);

            if (dbEntry == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbEntry.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            var viewModel = new EntryViewModel
            {
                Entry = dbEntry,
                EntryCategories = _entryCategoryRepo.GetActiveEntryCategories()
            };

            return View(viewModel);
        }

        public ActionResult Preview(Entry entry)
        {
            var dbEntry = _entryRepo.GetEntry(entry.Id);

            if (dbEntry == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbEntry.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            dbEntry.Paragraphs = _paragraphRepo.GetParagraphsByEntryId(entry.Id);

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
            GetSignedUser();

            _entryRepo.SaveEntry(entry, User.Identity.Name);

            return RedirectToAction("Index", "Entries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int entryId, string isVisible)
        {
            GetSignedUser();

            if(_signedUser == null)
                return HttpNotFound();

            if (!_signedUser.CanManageAllContent())
                return RedirectToAction("Index", "Home");

            var result = _entryRepo.PublishEntry(entryId, isVisible, User.Identity.Name);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "Entries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int entryId)
        {
            var entry = _entryRepo.GetEntry(entryId);

            if (entry == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !entry.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            _entryRepo.DeleteEntry(entryId);

            return RedirectToAction("Index", "Entries");
        }
    }
}