using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Accounts
{
    public class PasswordRecoveryQuestion
    {
        [Required(ErrorMessage = "Password recovery question is not selected")]
        public int Id { get; set; }
        public string Question { get; set; }
    }
}