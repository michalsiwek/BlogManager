using BlogManager.Models;
using BlogManager.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BlogManager.Repositories
{
    public interface IAccountRepository
    {
        Account GetSignedUser(string userIdentityName);
    }

    public class AccountRepository : IAccountRepository
    {
        public Account GetSignedUser(string userIdentityName)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Email.Equals(userIdentityName));
            }
        }
    }
}