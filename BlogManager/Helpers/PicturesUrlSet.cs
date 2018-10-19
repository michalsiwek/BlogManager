using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Helpers
{
    public class PicturesUrlSet
    {
        public string Body { get; set; }

        public List<Picture> GetPictures()
        {
            List<Picture> output = new List<Picture>();

            var splitBy = new char[] { '\r' };

            var urls = Body.Split(splitBy).ToList();
            foreach (var url in urls)
                output.Add(new Picture());


            return output;
        }
    }
}