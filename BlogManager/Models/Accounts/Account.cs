using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public bool IsVerified { get; set; }

        public AccountType AccountType { get; set; }
    }
}