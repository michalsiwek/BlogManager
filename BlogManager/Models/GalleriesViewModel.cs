using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Models
{
    public class GalleriesViewModel
    {
        public IEnumerable<Gallery> Galleries { get; set; }
    }
}