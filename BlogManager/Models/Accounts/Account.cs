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

        [Required]
        [StringLength(100)]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }

        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Display(Name = "Question")]
        public PasswordRecoveryQuestion PasswordRecoveryQuestion { get; set; }

        [Required]
        [Display(Name = "Answer")]
        public string PasswordRecoveryAnswer { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool CanManageAllContent()
        {
            if (AccountType.Name.Equals(AccountTypeName.Admin) || AccountType.Name.Equals(AccountTypeName.Editor))
                return true;
            return false;
        }
    }
}