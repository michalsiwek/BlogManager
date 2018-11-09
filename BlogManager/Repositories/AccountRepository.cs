using BlogManager.Models;
using BlogManager.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;

namespace BlogManager.Repositories
{
    public interface IAccountRepository
    {
        Account GetSignedUser(string userIdentityName);
        Account GetAccountById(int id);
        IEnumerable<Account> GetAllAccountsButAdmin();
        DbRepoStatusCode ValidateAccount(int accountId, string isActive);
        DbRepoStatusCode DeleteAccount(int accountId);
    }

    public class AccountRepository : IAccountRepository
    {
        private IAccountManageService _accountService;

        public AccountRepository()
        {
            _accountService = new AccountManageService();
        }

        public Account GetSignedUser(string userIdentityName)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Email.Equals(userIdentityName));
            }
        }

        public IEnumerable<Account> GetAllAccountsButAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users
                    .Include(u => u.AccountType)
                    .Where(u => u.Id != 1)
                    .ToList();
                return result;
            }
        }

        public Account GetAccountById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Id == id);
                return result;
            }
        }

        public DbRepoStatusCode ValidateAccount(int accountId, string isActive)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Id == accountId);

                if (account == null)
                    return DbRepoStatusCode.NotFound;

                if (account.AccountType == null)
                    return DbRepoStatusCode.BadRequest;

                _accountService.ValidateAccount(account, isActive);

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode DeleteAccount(int accountId)
        {
            using (var context = new ApplicationDbContext())
            {
                var account = context.Users.SingleOrDefault(u => u.Id == accountId);

                if (account == null)
                    return DbRepoStatusCode.NotFound;

                context.Users.Remove(account);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

    }
}