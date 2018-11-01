using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Accounts
{
    public class PasswordResetVerificationCode
    {
        public int Id { get; set; }

        public Account Account { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}