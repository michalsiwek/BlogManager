using BlogManager.Models.Categories;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class GalleryViewModel
    {
        public Gallery Gallery { get; set; }

        [Display(Name = "Category")]
        public IEnumerable<ContentCategory> ContentCategories { get; set; }

        [Display(Name = "Subcategory")]
        public IEnumerable<ContentSubcategory> ContentSubCategories { get; set; }

        public GalleryViewModel() { }
        public GalleryViewModel(Gallery gallery)
        {
            Gallery = gallery;
        }
    }
}