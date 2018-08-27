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
        [Display(Name = "Title")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public List<Paragraph> Paragraphs { get; set; }

        [Display(Name = "Image Url")]
        [Url]
        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public EntryCategory EntryCategory { get; set; }

        public Account LastModifiedBy { get; set; }


        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var temp = obj as Entry;

            if (this.EntryCategory.Id == temp.EntryCategory.Id &&
               this.Title.Equals(temp.Title) &&
               this.Description.Equals(temp.Description) &&
               this.Content.Equals(temp.Content))
            {
                if (ImageUrl == null)
                    return temp.ImageUrl == null;
                else
                    return ImageUrl.Equals(temp.ImageUrl);
            }
            else
                return false;
        }

        #endregion
    }
}