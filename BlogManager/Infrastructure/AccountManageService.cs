using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogManager.Models.Accounts;

namespace BlogManager.Infrastructure
{
    public interface IAccountManageService
    {
        void ValidateAccount(Account account, string isActive);
    }

    public class AccountManageService : IAccountManageService
    {
        public void ValidateAccount(Account account, string isActive)
        {
            account.LastModification = DateTime.Now;

            switch (isActive.ToLower())
            {
                case "true":
                    account.IsActive = true;
                    break;
                case "false":
                    account.IsActive = false;
                    break;
                default:
                    break;
            }
        }
    }
}