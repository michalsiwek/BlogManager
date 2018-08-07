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
        private ApplicationDbContext _context = new ApplicationDbContext();

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
        public ActionResult Create(EntryViewModel viewModel)
        {
            viewModel.Entry.CreateDate = DateTime.Now;
            viewModel.Entry.EntryCategory = _context.EntryCategories.SingleOrDefault(c => c.Id == viewModel.Entry.EntryCategory.Id);

            return View();
        }
    }
}