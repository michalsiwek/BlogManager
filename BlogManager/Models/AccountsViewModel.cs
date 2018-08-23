using BlogManager.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class AccountsViewModel
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
}