using BlogManager.Models.Entries;
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
        
        public static List<Paragraph> GetParagraphsFromContent(this Entry entry)
        {
            List<Paragraph> output = new List<Paragraph>();

            var temp = entry.Content;
            int subContentId = 0;

            temp = temp.Replace("\r\n\r\n", "|").Trim();
            string[] paragraphs = temp.Split('|');

            foreach (var p in paragraphs)
            {
                subContentId++;
                output.Add(new Paragraph()
                {
                    SubContentId = subContentId,
                    Entry_Id = entry.Id,
                    Body = p
                });
            }

            return output;
        }
    }
}