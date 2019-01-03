using BlogManager.Models.Accounts;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Galleries
{
    public class Gallery
    {
        public int Id { get; set; }

        public Account Account { get; set; }

        [Display(Name = "Add Pictures")]
        public List<Picture> Pictures { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        [Display(Name = "Is Visible")]
        public bool IsVisible { get; set; }

        public Account LastModifiedBy { get; set; }

        public DateTime? LastModification { get; set; }

        public ContentCategory ContentCategory { get; set; }

        public ContentSubcategory ContentSubcategory { get; set; }

    }
}