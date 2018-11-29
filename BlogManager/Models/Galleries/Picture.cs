using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Galleries
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        [StringLength(255)]
        public string Descripton { get; set; }
        [StringLength(50)]
        public string Author { get; set; }
        public string Url { get; set; }
        public string AbsoluteUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public int GalleryId { get; set; }

        public Picture() { }

        public Picture(string fileName, string url, string absoluteUrl, DateTime uploadDate)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            Url = url;
            AbsoluteUrl = absoluteUrl;
            UploadDate = uploadDate;
        }

        public Picture(string fileName, string url, string absoluteUrl, DateTime uploadDate, int galleryId)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            Url = url;
            AbsoluteUrl = absoluteUrl;
            UploadDate = uploadDate;
            GalleryId = galleryId;
        }
    }
}