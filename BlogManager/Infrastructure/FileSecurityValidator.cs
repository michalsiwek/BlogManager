using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogManager.Infrastructure
{
    public interface IFileSecurityValidator
    {
        bool FileSecurityValidation(HttpPostedFileBase file);
    }

    public class FileSecurityValidator : IFileSecurityValidator
    {
        public bool FileSecurityValidation(HttpPostedFileBase file)
        {
            var fileFormatValidation = file.FileName.EndsWith(".jpg") ||
                                       file.FileName.EndsWith(".jpeg") ||
                                       file.FileName.EndsWith(".png");
            var fileContentValidation = file.ContentType.StartsWith("image/");

            return (fileFormatValidation && fileContentValidation);
        }
    }
}