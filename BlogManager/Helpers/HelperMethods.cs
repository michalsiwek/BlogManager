using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BlogManager.Helpers
{
    public static class HelperMethods
    {
        public static string ConvertToHtmlParagraps(this string content)
        {
            var output = content.NormalizeContent();
            output = output.Replace("\r\n\r\n", "</p>\n<p>");
            output = "<p>" + output + "</p>";
            return output;
        }
        
        public static List<Paragraph> GetParagraphsFromContent(this Entry entry)
        {
            List<Paragraph> output = new List<Paragraph>();

            var temp = entry.Content.NormalizeContent();
            int subContentId = 0;

            temp = temp.Replace("\r\n\r\n", "|").Trim();
            string[] paragraphs = temp.Split('|');

            foreach (var p in paragraphs)
            {
                subContentId++;
                output.Add(new Paragraph()
                {
                    SubContentId = subContentId,
                    EntryId = entry.Id,
                    Body = p
                });
            }

            return output;
        }

        public static string NormalizeContent(this string content)
        {
            var output = content.Trim();
            while(output.Contains("\r\n\r\n\r\n"))
                output = output.Replace("\r\n\r\n\r\n", "\r\n\r\n");
            return output;
        }

    }
}
