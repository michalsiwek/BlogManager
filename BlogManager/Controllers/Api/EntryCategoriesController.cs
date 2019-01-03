using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlogManager.Controllers.Api
{
    public class ContentCategoriesController : ApiController
    {
        private ApplicationDbContext _context;

        public ContentCategoriesController()
        {
            _context = new ApplicationDbContext();
        }

        /*[HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteCategory(int id)
        {
            var catFromDb = _context.ContentCategories.SingleOrDefault(c => c.Id == id && c.Id != 1);

            if (catFromDb == null)
                return NotFound();

            _context.ContentCategories.Remove(catFromDb);
            _context.SaveChanges();

            return Ok();
        }*/
    }
}
