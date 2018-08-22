using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Helpers
{
    public static class HelperMethods
    {
        public static string ConvertToHtmlParagraps(this string content)
        {
            var temp = content;
            temp = temp.Replace("\r\n\r\n", "</p>\n<p>").Trim();
            temp = "<p>" + temp + "</p>";
            return temp;
        }
    }
}