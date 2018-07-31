using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BlogManager.Models.Accounts;
using System.ComponentModel.DataAnnotations;
using System;
using BlogManager.Models.Entries;
using BlogManager.Models.Categories;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;

namespace BlogManager.Models
{
    public class CustomUserRole : IdentityUserRole<int> { }

    public class CustomRoleStore : RoleStore<AccountType, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context) { }
    }

    public class ApplicationRoleManager : RoleManager<AccountType, int>
    {
        public ApplicationRoleManager(IRoleStore<AccountType, int> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<AccountType, int, CustomUserRole>(context.Get<ApplicationDbContext>()));
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryCategory> EntryCategories { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}