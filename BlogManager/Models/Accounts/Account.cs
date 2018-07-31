using Microsoft.AspNet.Identity.EntityFramework;
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
        public DateTime? ModifyDate { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Surname { get; set; }

        public AccountType AccountType { get; set; }
    }
}