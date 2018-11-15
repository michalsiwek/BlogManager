using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BlogManager.Models.Galleries;

namespace BlogManager.Infrastructure
{
    public interface IFileUploadService
    {
        Picture SaveFile(HttpPostedFileBase file, HttpServerUtilityBase server, int galleryId);
        void DeleteDirectory(HttpServerUtilityBase server, string directoryName);
        void DeleteFile(HttpServerUtilityBase server, string dirName, string fileName);
    }

    public class FileUploadService : IFileUploadService
    {
        private IFileSecurityValidator _fileSecurityValidator;

        public FileUploadService()
        {
            _fileSecurityValidator = new FileSecurityValidator();
        }

        public Picture SaveFile(HttpPostedFileBase file, HttpServerUtilityBase server, int galleryId)
        {
            var fileName = Path.GetFileName(file.FileName);
            var dirPath = server.MapPath($"~/Pictures/{galleryId}");
            Directory.CreateDirectory(dirPath);
            var serverPath = Path.Combine(dirPath + "\\", fileName);
            var dbPath = $"/Pictures/{galleryId}/{Path.GetFileName(file.FileName)}";

            if (File.Exists(serverPath))
                return null;

            file.SaveAs(serverPath);

            return new Picture(file.FileName, dbPath, serverPath, DateTime.Now);
        }

        public void DeleteDirectory(HttpServerUtilityBase server, string directoryName)
        {
            var dirPath = server.MapPath($"~/Pictures/{directoryName}");
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
        }

        public void DeleteFile(HttpServerUtilityBase server, string dirName, string fileName)
        {
            var dirPath = server.MapPath(string.Format("~/Pictures/{0}", dirName));
            var serverPath = Path.Combine(dirPath + "\\", fileName);

            if (File.Exists(serverPath))
                File.Delete(serverPath);
        }

    }
}