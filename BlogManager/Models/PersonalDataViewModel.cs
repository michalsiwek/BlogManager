using BlogManager.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class PersonalDataViewModel
    {
        public Account Account { get; set; }
    }
}