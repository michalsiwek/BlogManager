using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntriesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var viewModel = new EntriesViewModel
            {
                Entries = _context.Entries  // maybe Join
                    .Include("AspNetUsers")
                    .Include("EntryCategories")
                    .ToList()
            };
            return View(viewModel);
        }
    }
}