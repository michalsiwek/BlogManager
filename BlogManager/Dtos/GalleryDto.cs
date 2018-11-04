using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Dtos
{
    public class GalleryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<PictureDto> Pictures { get; set; }

        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModification { get; set; }
    }
}