using BlogManager.Models;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogManager.Helpers.Enums;
using BlogManager.Repositories;
using BlogManager.Infrastructure;

namespace BlogManager.Controllers
{
    [Authorize(Roles = AccountTypeName.Admin + "," + AccountTypeName.Editor)]
    public class EntryCategoriesController : Controller
    {
        private readonly IEntryCategoryRepository _repo;

        public EntryCategoriesController()
        {
            _repo = new EntryCategoryRepository(new EntryCategoryManageService());
        }

        public ActionResult Index()
        {
            var viewModel = new EntryCategoriesViewModel
            {
                EntryCategories = _repo.GetEntryCategories()
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
            var dbEntryCat = _repo.GetEntryCategory(id);

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
            _repo.SaveEntryCategory(entryCategory);

            return RedirectToAction("Index", "EntryCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, string isActive)
        {
            var result =_repo.ActivateEntryCategory(id, isActive);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "EntryCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var result = _repo.DeleteEntryCategory(id);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "EntryCategories");
        }
    }
}