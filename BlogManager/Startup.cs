﻿using BlogManager.Models;
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
            var roleManager = new ApplicationRoleManager(new CustomRoleStore(context));

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
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roleManager = new ApplicationRoleManager(new CustomRoleStore(context));

            var accountTypeName = "Admin";

            var account = new Account();
            account.Email = "admin@blogmanager.com";
            account.UserName = account.Email;
            account.CreateDate = DateTime.Now;
            account.LockoutEnabled = false;
            account.AccountType = roleManager.FindByName(accountTypeName);

            var accountPassword = "admin1";

            var checkUser = userManager.Create(account, accountPassword);

            if(checkUser.Succeeded)
                userManager.AddToRole(account.Id, accountTypeName);

        }
    }
}