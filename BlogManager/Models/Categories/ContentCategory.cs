using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Categories
{
    public class ContentCategory
    {
        [Required(ErrorMessage = "Category is not selected")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public List<ContentSubcategory> Subcategories { get; set; }
    }
}