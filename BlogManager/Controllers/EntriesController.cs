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
            var entries = new EntriesViewModel
            {
                Entries = _context.Entries.ToList()
            };
            return View(entries);
        }
    }
}