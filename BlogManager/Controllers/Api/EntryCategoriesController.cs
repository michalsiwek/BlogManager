using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlogManager.Controllers.Api
{
    public class EntryCategoriesController : ApiController
    {
        private ApplicationDbContext _context;

        public EntryCategoriesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteCategory(int id)
        {
            var catFromDb = _context.EntryCategories.SingleOrDefault(c => c.Id == id && c.Id != 1);

            if (catFromDb == null)
                return NotFound();

            _context.EntryCategories.Remove(catFromDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
