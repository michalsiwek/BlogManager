﻿using BlogManager.Models.Accounts;
using BlogManager.Models.Categories;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class EntryViewModel
    {
        public Entry Entry { get; set; }

        [Display(Name = "Category")]
        public IEnumerable<EntryCategory> EntryCategories { get; set; }

        public Account LastModifiedBy { get; set; }
    }
}