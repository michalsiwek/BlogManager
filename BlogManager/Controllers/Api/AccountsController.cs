using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlogManager.Controllers.Api
{
    public class AccountsController : ApiController
    {
        private ApplicationDbContext _context;

        public AccountsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteAccount(int id)
        {
            var userFromDb = _context.Users.SingleOrDefault(u => u.Id == id && u.Id != 1);

            if (userFromDb == null)
                return NotFound();

            _context.Users.Remove(userFromDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
