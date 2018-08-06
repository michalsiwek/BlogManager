﻿using BlogManager.Models.Accounts;
using BlogManager.Models.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class EntryViewModel
    {
        public int Id { get; set; }

        [Required]
        public Account Account { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public EntryCategory EntryCategory { get; set; }

        public Account LastModifiedBy { get; set; }
    }
}