﻿using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;

namespace BlogManager.Repositories
{
    public interface IGalleryRepository
    {
        int GetNewestGalleryId();
        int GetRecentCreatedGalleryIdByAccount(Gallery gallery);
        IEnumerable<Gallery> GetGalleries();
        Gallery GetGalleryById(int id);
        List<Picture> GetGalleryPictures(int galleryId);
        Picture GetPictureById(Guid pictureId);
        DbRepoStatusCode SaveGalleryChanges(Gallery gallery, string email, HttpRequestBase request, HttpServerUtilityBase server);
        DbRepoStatusCode ValidateGallery(int galleryId, string isVisible);
        DbRepoStatusCode DeleteGallery(int galleryId, string email);
        DbRepoStatusCode SaveNewPictures(Gallery gallery, string email, HttpRequestBase request, HttpServerUtilityBase server);
        DbRepoStatusCode SavePictureChanges(Picture picture, string email);
        DbRepoStatusCode DeletePicture(Guid pictureId, string email, HttpServerUtilityBase server);
    }

    public class GalleryRepository : IGalleryRepository
    {
        private IFileSecurityValidator _fileSecurityValidator;
        private IFileUploadService _fileUploadService;

        public GalleryRepository(IFileSecurityValidator fileSecurityValidator, IFileUploadService fileUploadService)
        {
            _fileSecurityValidator = fileSecurityValidator;
            _fileUploadService = fileUploadService;
        }

        public int GetNewestGalleryId()
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Database
                    .SqlQuery<int>(@"SELECT MAX(Id) FROM dbo.Galleries")
                    .ToList()
                    .First();
                return output;
            }
        }

        public int GetRecentCreatedGalleryIdByAccount(Gallery gallery)
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Database
                    .SqlQuery<int>(string.Format(@"SELECT MAX(Id) FROM dbo.Galleries 
                                        WHERE Account_Id = {0}
                                            AND ContentCategory_Id = {1}
                                            AND DATEADD(ss, 30, CreateDate) > GETDATE()", gallery.Account.Id, gallery.ContentCategory.Id))
                    .ToList()
                    .First();
                return output;
            }
        }

        public IEnumerable<Gallery> GetGalleries()
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Galleries.Include(e => e.Account).ToList();
                return output;
            }
        }

        public Gallery GetGalleryById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Galleries
                    .Include(g => g.Pictures)
                    .Include(g => g.Account)
                    .Include(g => g.ContentCategory)
                    .Include(g => g.ContentSubcategory)
                    .SingleOrDefault(g => g.Id == id);
                return output;
            }
        }

        public List<Picture> GetGalleryPictures(int galleryId)
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Pictures.Where(p => p.GalleryId == galleryId).ToList();
                return output;
            }
        }

        public Picture GetPictureById(Guid pictureId)
        {
            using (var context = new ApplicationDbContext())
            {
                var output = context.Pictures.SingleOrDefault(p => p.Id == pictureId);
                return output;
            }
        }

        public DbRepoStatusCode SaveGalleryChanges(Gallery gallery, string email, HttpRequestBase request, HttpServerUtilityBase server)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbGallery = context.Galleries
                .Include(g => g.ContentCategory)
                .Include(g => g.ContentSubcategory)
                .Include(g => g.Pictures)
                .SingleOrDefault(g => g.Id == gallery.Id);

                var contentCat = context.ContentCategories.Single(c => c.Id == gallery.ContentCategory.Id);
                var contentSubcat = context.ContentSubcategories.SingleOrDefault(c => c.Id == gallery.ContentSubcategory.Id);

                gallery.Pictures = new List<Picture>();

                if (dbGallery == null)
                {
                    gallery.CreateDate = DateTime.Now;
                    gallery.Account = context.Users.SingleOrDefault(u => u.Email.Equals(email));
                    gallery.IsVisible = false;
                    gallery.ContentCategory = contentCat;
                    gallery.ContentSubcategory = contentSubcat;

                    context.Galleries.Add(gallery);
                    context.SaveChanges();

                    if (request.Files.Count > 0 && request.Files[0].ContentLength > 0)
                    {
                        var galleryId = GetRecentCreatedGalleryIdByAccount(gallery);

                        for (var i = 0; i < request.Files.Count; i++)
                        {
                            var file = request.Files[i];

                            if (!_fileSecurityValidator.FileSecurityValidation(file))
                                continue;

                            var picture = _fileUploadService.SaveFile(file, server, galleryId);

                            if(picture != null) 
                                gallery.Pictures.Add(picture);
                        }

                        foreach (var p in gallery.Pictures)
                            context.Pictures.Add(p);
                    }
                }
                else
                {
                    var signedUser = context.Users
                        .Include(u => u.AccountType)
                        .SingleOrDefault(u => u.Email.Equals(email));

                    if (signedUser == null)
                        return DbRepoStatusCode.NotFound;

                    if (!signedUser.CanManageAllContent() && !dbGallery.Account.Equals(signedUser))
                        return DbRepoStatusCode.BadRequest;

                    dbGallery.Title = gallery.Title;
                    dbGallery.Description = gallery.Description;
                    dbGallery.IsVisible = gallery.IsVisible;
                    dbGallery.LastModification = DateTime.Now;
                    dbGallery.LastModifiedBy = signedUser;
                    dbGallery.ContentCategory = contentCat;
                    dbGallery.ContentSubcategory = contentSubcat;
                }

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode ValidateGallery(int galleryId, string isVisible)
        {
            using (var context = new ApplicationDbContext())
            {
                var galleryToValidate = context.Galleries.SingleOrDefault(e => e.Id == galleryId);
                if (galleryToValidate == null)
                    return DbRepoStatusCode.NotFound;

                switch (isVisible.ToLower())
                {
                    case "true":
                        galleryToValidate.IsVisible = true;
                        break;
                    case "false":
                        galleryToValidate.IsVisible = false;
                        break;
                    default:
                        break;
                }

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode DeleteGallery(int galleryId, string email)
        {
            using (var context = new ApplicationDbContext())
            {
                var galleryToDelete = context.Galleries.SingleOrDefault(e => e.Id == galleryId);
                if (galleryToDelete == null)
                    return DbRepoStatusCode.NotFound;

                var signedUser = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Email.Equals(email));

                if (signedUser == null)
                    return DbRepoStatusCode.BadRequest;

                var pictures = context.Pictures.Where(p => p.GalleryId == galleryId).ToList();

                context.Pictures.RemoveRange(pictures);
                context.Galleries.Remove(galleryToDelete);
                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode SaveNewPictures(Gallery gallery, string email, HttpRequestBase request, HttpServerUtilityBase server)
        {
            using (var context = new ApplicationDbContext())
            {
                var filenameConflict = false;

                var dbGallery = context.Galleries
                    .Include(g => g.Pictures)
                    .SingleOrDefault(g => g.Id == gallery.Id);

                if (dbGallery == null)
                    return DbRepoStatusCode.NotFound;

                gallery.Pictures = new List<Picture>();

                if (request.Files.Count > 0 && request.Files[0].ContentLength > 0)
                {
                    for (var i = 0; i < request.Files.Count; i++)
                    {
                        var file = request.Files[i];

                        if (!_fileSecurityValidator.FileSecurityValidation(file))
                            continue;

                        var picture = _fileUploadService.SaveFile(file, server, dbGallery.Id);

                        if (picture == null)
                        {
                            filenameConflict = true;
                            continue;
                        }

                        dbGallery.Pictures.Add(picture);
                    }

                    foreach (var p in gallery.Pictures)
                        context.Pictures.Add(p);
                }

                var signedUser = context.Users
                    .Include(u => u.AccountType)
                    .SingleOrDefault(u => u.Email.Equals(email));

                if (signedUser == null)
                    return DbRepoStatusCode.BadRequest;

                dbGallery.LastModification = DateTime.Now;
                dbGallery.LastModifiedBy = signedUser;

                context.SaveChanges();

                if (filenameConflict)
                    return DbRepoStatusCode.PartialSuccess;

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode SavePictureChanges(Picture picture, string email)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbPicture = context.Pictures.SingleOrDefault(p => p.Id == picture.Id);
                var dbGallery = context.Galleries.SingleOrDefault(g => g.Id == dbPicture.GalleryId);

                if (dbPicture == null || dbGallery == null)
                    return DbRepoStatusCode.NotFound;

                var account = context.Users
                    .Include(c => c.AccountType)
                    .SingleOrDefault(a => a.Email.Equals(email));

                if (!account.CanManageAllContent() && !dbGallery.Account.Equals(account))
                    return DbRepoStatusCode.BadRequest;

                dbPicture.Author = picture.Author;
                dbPicture.Descripton = picture.Descripton;

                dbGallery.LastModification = DateTime.Now;
                dbGallery.LastModifiedBy = account;

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }

        public DbRepoStatusCode DeletePicture(Guid pictureId, string email, HttpServerUtilityBase server)
        {
            using (var context = new ApplicationDbContext())
            {
                var picToDelete = context.Pictures.SingleOrDefault(p => p.Id == pictureId);
                var dbGallery = context.Galleries.SingleOrDefault(g => g.Id == picToDelete.GalleryId);

                if (picToDelete == null || dbGallery == null)
                    return DbRepoStatusCode.NotFound;

                var account = context.Users
                    .Include(a => a.AccountType)
                    .SingleOrDefault(a => a.Email.Equals(email));
                if (!account.CanManageAllContent() && !dbGallery.Account.Equals(account))
                    return DbRepoStatusCode.BadRequest;

                context.Pictures.Remove(picToDelete);

                dbGallery.LastModification = DateTime.Now;
                dbGallery.LastModifiedBy = account;

                context.SaveChanges();

                return DbRepoStatusCode.Success;
            }
        }
    }
}