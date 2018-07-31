using BlogManager.Models.Categories;
using BlogManager.Models.Accounts;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Entries
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public Account Account { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public EntryCategory EntryCategory { get; set; }
    }
}