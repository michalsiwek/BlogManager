using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Accounts
{
    public class AccountType : IdentityRole
    {
        public string Description { get; set; }

        public AccountType() { }

        public AccountType(string accountTypeName)
        {
            this.Name = accountTypeName;
        }
    }
}