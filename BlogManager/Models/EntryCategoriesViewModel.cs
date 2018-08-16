using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class EntryCategoriesViewModel
    {
        public IEnumerable<EntryCategory> EntryCategories { get; set; }
    }
}