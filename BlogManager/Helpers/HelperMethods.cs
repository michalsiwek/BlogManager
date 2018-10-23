using BlogManager.Models;
using BlogManager.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BlogManager.Models.Galleries;

namespace BlogManager.Helpers
{
    public static class HelperMethods
    {
        public static string UnPolish(this string text)
        {
            return text.Replace("ą", "a")
                .Replace("ę", "e")
                .Replace("ć", "c")
                .Replace("ł", "l")
                .Replace("ó", "o")
                .Replace("ń", "n")
                .Replace("ś", "s")
                .Replace("ź", "z")
                .Replace("ż", "z");
        }

    }
}
