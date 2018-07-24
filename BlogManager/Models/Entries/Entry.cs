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

        public Account Account { get; set; }
        public int AccountId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public EntryCategory EntryCategory { get; set; }
    }
}