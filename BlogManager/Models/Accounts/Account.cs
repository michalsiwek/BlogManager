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
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

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