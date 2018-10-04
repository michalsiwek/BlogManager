using BlogManager.Models.Accounts;
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
        public List<Picture> Pictures { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsVisible { get; set; }
    }
}