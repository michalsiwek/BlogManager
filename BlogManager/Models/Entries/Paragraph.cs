using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models.Entries
{
    public class Paragraph
    {
        public int Id { get; set; }
        public int Entry_Id { get; set; }
        public int SubContentId { get; set; }
        public string Body { get; set; }
    }
}