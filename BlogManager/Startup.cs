using BlogManager.Models;
using BlogManager.Models.Accounts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(BlogManager.Startup))]
namespace BlogManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
            CreateAdminAccount();
        }

        private ApplicationDbContext context = new ApplicationDbContext();

        private void CreateRoles()
        {
            var roleManager = new RoleManager<AccountType>(new RoleStore<AccountType>(context));

            var startUpAccountTypes = new List<string> { "Admin", "Editor", "Guest" };

            foreach (var type in startUpAccountTypes)
            {
                if (!roleManager.RoleExists(type))
                {
                    var accountType = new AccountType();
                    accountType.Name = type;
                    accountType.Description = type + " Access";
                    roleManager.Create(accountType);
                }
            }

        }

        private void CreateAdminAccount()
        {
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roleManager = new RoleManager<AccountType>(new RoleStore<AccountType>(context));

            var accountTypeName = "Admin";

            var account = new Account();
            account.UserName = "admin";
            account.Email = "admin@blogmanager.com";
            account.CreateDate = DateTime.Now;
            account.LockoutEnabled = false;
            account.AccountType = roleManager.FindByName(accountTypeName);

            var accountPassword = "Admin1@";

            var checkUser = userManager.Create(account, accountPassword);

            if(checkUser.Succeeded)
                userManager.AddToRole(account.Id, accountTypeName);

        }
    }
}