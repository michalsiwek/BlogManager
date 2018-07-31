using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Accounts
{
    public class AccountType : IdentityRole<int, CustomUserRole>
    {
        new public int Id { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        public AccountType() : base() { }

    }
}