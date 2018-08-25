using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BlogManager.Models.Accounts
{
    public class Account : ApplicationUser
    {
        [Required]
        [Display(Name = "Created")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        [StringLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}