using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class EntryController : Controller
    {
        // GET: Entry
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}