using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class EntriesViewModel
    {
        public IEnumerable<Entry> Entries { get; set; }
    }
}