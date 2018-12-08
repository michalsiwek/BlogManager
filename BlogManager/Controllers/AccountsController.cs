using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogManager.Helpers.Enums;
using BlogManager.Repositories;
using BlogManager.Infrastructure;

namespace BlogManager.Controllers
{
    [Authorize(Roles = AccountTypeName.Admin)]
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _accountRepo;

        public AccountsController()
        {
            _accountRepo = new AccountRepository(new AccountManageService(), new MailingService());
        }

        public ActionResult Index()
        {
            var viewModel = new AccountsViewModel
            {
                Accounts = _accountRepo.GetAllAccountsButAdmin()
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int accountId, string isActive, AccountsViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var result = _accountRepo.ValidateAccount(accountId, isActive);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Forbidden action");

            return RedirectToAction("Index", "Accounts");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var result = _accountRepo.DeleteAccount(id);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "Accounts");
        }

    }
}