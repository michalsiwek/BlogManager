using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class ContentCategoriesViewModel
    {
        public IEnumerable<ContentCategory> ContentCategories { get; set; }
    }
}