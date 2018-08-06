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

        private ApplicationDbContext _context = new ApplicationDbContext();

        private void CreateRoles()
        {
            var roleManager = new RoleManager<AccountType, int>(new CustomRoleStore(_context));

            var startUpAccountTypes = new List<string> { "Admin", "Editor", "Guest" };

            foreach (var type in startUpAccountTypes)
            {
                if (!roleManager.RoleExists(type))
                {
                    var accountType = new AccountType();
                    accountType.Name = type;
                    accountType.Description = type + " Access";
                    accountType.CreateDate = DateTime.Now;
                    roleManager.Create(accountType);
                }
            }

        }

        private void CreateAdminAccount()
        {
            var roleManager = new RoleManager<AccountType, int>(new CustomRoleStore(_context));
            var userManager = new ApplicationUserManager(new CustomUserStore(_context));

            var accountTypeName = "Admin";
            var accountLogin = "admin@blogmanager.com";
            var accountPassword = "admin1";

            var account = new Account
            {
                Email = accountLogin,
                UserName = accountLogin,
                CreateDate = DateTime.Now,
                LockoutEnabled = false,
                IsActive = true,
                AccountType = roleManager.FindByName(accountTypeName)
            };

            var checkUser = userManager.Create(account, accountPassword);

            if(checkUser.Succeeded)
                userManager.AddToRole(account.Id, accountTypeName);

        }
    }
}