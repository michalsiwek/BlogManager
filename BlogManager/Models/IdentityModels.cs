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
using BlogManager.Models.Galleries;

namespace BlogManager.Models
{
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }


    public class CustomUserStore : UserStore<Account, AccountType, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context) { }
    }

    public class CustomRoleStore : RoleStore<AccountType, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context) { }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<Account, AccountType, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryCategory> EntryCategories { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<PasswordRecoveryQuestion> PasswordRecoveryQuestions { get; set; }
        public DbSet<PasswordResetVerificationCode> PasswordResetVerificationCodes { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}