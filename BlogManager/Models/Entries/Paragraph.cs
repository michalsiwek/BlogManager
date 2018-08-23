using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Entries
{
    public class Paragraph
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int SubContentId { get; set; }
        public string Body { get; set; }
    }
}