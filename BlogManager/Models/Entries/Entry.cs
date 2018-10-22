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

        public void NormalizeContent()
        {
            var output = Content.Trim();
            while (output.Contains("\r\n\r\n\r\n"))
                output = output.Replace("\r\n\r\n\r\n", "\r\n\r\n");
            Content = output;
        }

        public void NormalizeEntry()
        {
            Title = Title.Trim();
            Description = Description.Trim();
            NormalizeContent();
        }

        public void GetParagraphsFromContent()
        {
            List<Paragraph> output = new List<Paragraph>();

            NormalizeContent();
            var temp = Content;
            int subContentId = 0;

            temp = temp.Replace("\r\n\r\n", "|").Trim();
            string[] paragraphs = temp.Split('|');

            foreach (var p in paragraphs)
            {
                subContentId++;
                output.Add(new Paragraph()
                {
                    SubContentId = subContentId,
                    EntryId = Id,
                    Body = p
                });
            }

            Paragraphs = output;
        }

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var temp = obj as Entry;

            if (EntryCategory.Id == temp.EntryCategory.Id &&
               Title.Equals(temp.Title) &&
               Description.Equals(temp.Description) &&
               Content.Equals(temp.Content))
            {
                if (ImageUrl == null)
                    return temp.ImageUrl == null;
                return ImageUrl.Equals(temp.ImageUrl);
            }

            return false;
        }

        #endregion
    }
}