using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BlogManager.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext _context;

        public AccountsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var viewModel = new AccountsViewModel
            {
                Accounts = _context.Users.Where(u => u.AccountType.Id != 1).ToList()
            };
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Validate(int accountId, string isActive, AccountsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accountToValidate = _context.Users.SingleOrDefault(u => u.Id == accountId);
            if (accountToValidate == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if(account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            if (accountToValidate.AccountType == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Forbidden action");

            accountToValidate.LastModification = DateTime.Now;

            switch (isActive.ToLower())
            {
                case "true":
                    accountToValidate.IsActive = true;
                    _context.SaveChanges();
                    break;
                case "false":
                    accountToValidate.IsActive = false;
                    _context.SaveChanges();
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "Accounts");
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var accountToValidate = _context.Users.SingleOrDefault(u => u.Id == id);
            if (accountToValidate == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if (account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            _context.Users.Remove(accountToValidate);
            _context.SaveChanges();

            return RedirectToAction("Index", "Accounts");
        }

    }
}