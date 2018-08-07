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
        // GET: Entry
        public ActionResult New()
        {
            var entryCategories = _context.EntryCategories.ToList();

            var viewModel = new EntryViewModel
            {
                //Id = null,
                //Account = null,
                //CreateDate = DateTime.Now,
                //LastModification,
                //Title
                //Description
                //Content
                //ImageUrl
                //IsVisible
                Entry = new Entry(),
                EntryCategories = entryCategories
                //LastModifiedBy
            };

            return View(viewModel);
        }
    }
}