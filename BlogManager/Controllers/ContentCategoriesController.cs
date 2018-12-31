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
using System.Data.Entity;

namespace BlogManager.Controllers
{
    [Authorize(Roles = AccountTypeName.Admin + "," + AccountTypeName.Editor)]
    public class ContentCategoriesController : Controller
    {
        private readonly IContentCategoryRepository _repo;

        public ContentCategoriesController()
        {
            _repo = new ContentCategoryRepository(new ContentCategoryManageService());
        }

        public ActionResult Index()
        {
            var viewModel = new ContentCategoriesViewModel
            {
                ContentCategories = _repo.GetContentCategories()
            };
            return View(viewModel);
        }

        public ActionResult New()
        {
            ContentCategoryViewModel viewModel = new ContentCategoryViewModel
            {
                ContentCategory = new ContentCategory()
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var dbEntryCat = _repo.GetContentCategory(id);

            if (dbEntryCat == null)
                return HttpNotFound();

            if (dbEntryCat.Id == 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Forbidden action");

            var viewModel = new ContentCategoryViewModel
            {
                ContentCategory = dbEntryCat
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ContentCategory contentCategory)
        {
            _repo.SaveContentCategory(contentCategory);

            return RedirectToAction("Index", "ContentCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id, string isActive)
        {
            var result =_repo.ActivateContentCategory(id, isActive);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "ContentCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var result = _repo.DeleteContentCategory(id);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "ContentCategories");
        }

        public ActionResult NewSubcategory(int contentCategoryId)
        {
            ContentSubcategoryViewModel viewModel = new ContentSubcategoryViewModel()
            {
                ContentCategoryId = contentCategoryId,
                ContentSubcategory = new ContentSubcategory()
            };

            return PartialView("_SubcategoryForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewSubcategory(ContentSubcategoryViewModel model)
        {
            _repo.SaveContentSubcategory(model);

            return RedirectToAction("Edit", "ContentCategories", new { id = model.ContentCategoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubcategory(int id)
        {
            var result = _repo.DeleteContentSubcategory(id);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            int categoryId = 200;

            return RedirectToAction("Edit", "ContentCategories", new { id = categoryId });
        }
    }
}