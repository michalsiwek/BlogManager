using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlogManager.Models.Categories;

namespace BlogManager.Models
{
    public class ContentSubcategoryViewModel
    {
        public int ContentCategoryId { get; set; }
        public ContentSubcategory ContentSubcategory { get; set; }
    }
}