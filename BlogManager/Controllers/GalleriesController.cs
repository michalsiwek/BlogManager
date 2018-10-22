using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using BlogManager.Models.Accounts;
using BlogManager.Repositories;

namespace BlogManager.Controllers
{
    public class GalleriesController : Controller
    {
        private ApplicationDbContext _context;
        private DbRepository _dbRepository;
        private Account _signedUser;

        public GalleriesController()
        {
            _context = new ApplicationDbContext();
            _dbRepository = new DbRepository();
            _signedUser = new Account();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        private void GetSignedUser()
        {
            _signedUser = _context.Users
                .Include(u => u.AccountType)
                .SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new GalleriesViewModel
            {
                Galleries = _context.Galleries.Include(e => e.Account).ToList()
            };

            GetSignedUser();
            if (!_signedUser.CanManageAllContent())
                viewModel.Galleries = viewModel.Galleries.Where(e => e.Account.Equals(_signedUser)).ToList();

            return View(viewModel);
        }

        public ActionResult New()
        {
            var viewModel = new GalleryViewModel
            {
                Gallery = new Gallery()
            };

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();

            var dbGallery = _context.Galleries
                .Include(g => g.Pictures)
                .Include(g => g.Account)
                .SingleOrDefault(g => g.Id == id);

            if (dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            var viewModel = new GalleryViewModel(dbGallery);

            viewModel.Gallery.Pictures = _context.Pictures.Where(p => p.GalleryId == id).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Gallery gallery)
        {
            var dbGallery = _context.Galleries
                .Include(g => g.Pictures)
                .SingleOrDefault(g => g.Id == gallery.Id);

            gallery.Pictures = new List<Picture>();

            if (dbGallery == null)
            {
                gallery.CreateDate = DateTime.Now;
                gallery.Account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                gallery.IsVisible = false;

                _context.Galleries.Add(gallery);
                _context.SaveChanges();

                if(Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var galleryId = _dbRepository.GetRecentCreatedGalleryIdByAccount(gallery);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        var dirPath = Server.MapPath(string.Format("~/Pictures/{0}", galleryId));
                        Directory.CreateDirectory(dirPath);
                        var serverPath = Path.Combine(dirPath + "\\", fileName);
                        var dbPath = string.Format("/Pictures/{0}/{1}", galleryId, fileName);
                        file.SaveAs(serverPath);
                        gallery.Pictures.Add(new Picture(file.FileName, dbPath, DateTime.Now));
                    }

                    foreach (var p in gallery.Pictures)
                        _context.Pictures.Add(p);
                }
                
            }
            else
            {
                GetSignedUser();
                if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                    return RedirectToAction("Index", "Home");

                dbGallery.Title = gallery.Title;
                dbGallery.Description = gallery.Description;
                dbGallery.IsVisible = gallery.IsVisible;
                dbGallery.LastModification = DateTime.Now;
                dbGallery.LastModifiedBy = _signedUser;
            }            

            _context.SaveChanges();

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int galleryId, string isVisible)
        {
            GetSignedUser();
            if (!_signedUser.CanManageAllContent())
                return RedirectToAction("Index", "Home");

            var galleryToValidate = _context.Galleries.SingleOrDefault(e => e.Id == galleryId);
            if (galleryToValidate == null)
                return HttpNotFound();

            switch (isVisible.ToLower())
            {
                case "true":
                    galleryToValidate.IsVisible = true;
                    _context.SaveChanges();
                    break;
                case "false":
                    galleryToValidate.IsVisible = false;
                    _context.SaveChanges();
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int galleryId)
        {
            var galleryToDelete = _context.Galleries.SingleOrDefault(e => e.Id == galleryId);
            if (galleryToDelete == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !galleryToDelete.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            _context.Galleries.Remove(galleryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPictures(Gallery gallery)
        {
            ModelState.Clear();
            var filenameConflict = false;
            int filenameConflictsCount = 0;

            var dbGallery = _context.Galleries
                .Include(g => g.Pictures)
                .SingleOrDefault(g => g.Id == gallery.Id);

            gallery.Pictures = new List<Picture>();

            if (dbGallery == null)
                return HttpNotFound();
            
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    var dirPath = Server.MapPath(string.Format("~/Pictures/{0}", dbGallery.Id));
                    var serverPath = Path.Combine(dirPath + "\\", fileName);
                    var dbPath = string.Format("/Pictures/{0}/{1}", dbGallery.Id, fileName);

                    if (System.IO.File.Exists(serverPath))
                    {
                        filenameConflict = true;
                        filenameConflictsCount++;
                        continue;
                    }

                    file.SaveAs(serverPath);
                    gallery.Pictures.Add(new Picture(file.FileName, dbPath, DateTime.Now, dbGallery.Id));
                }

                foreach (var p in gallery.Pictures)
                    _context.Pictures.Add(p);
            }

            dbGallery.LastModification = DateTime.Now;
            dbGallery.LastModifiedBy = _signedUser;

            _context.SaveChanges();

            var viewModel = new GalleryViewModel
            {
                Gallery = _context.Galleries
                    .Include(g => g.Pictures)
                    .SingleOrDefault(g => g.Id == gallery.Id)
            };

            if(filenameConflict)
                ModelState.AddModelError("", $"{filenameConflictsCount} file(s) upload failed due to filename conflict");

            return View("Edit", viewModel);
        }

        public ActionResult PictureEdit(Guid id)
        {
            var dbPicture = _context.Pictures.SingleOrDefault(p => p.Id == id);
            var dbGallery = _context.Galleries.SingleOrDefault(g => g.Id == dbPicture.GalleryId);

            if (dbPicture == null || dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            PictureViewModel viewModel = new PictureViewModel();
            viewModel.Picture = dbPicture;

            return PartialView("_PictureEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PictureSaveChanges(Picture picture)
        {
            var dbPicture = _context.Pictures.SingleOrDefault(p => p.Id == picture.Id);
            var dbGallery = _context.Galleries.SingleOrDefault(g => g.Id == dbPicture.GalleryId);

            if (dbPicture == null || dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            dbPicture.Author = picture.Author;
            dbPicture.Descripton = picture.Descripton;

            dbGallery.LastModification = DateTime.Now;
            dbGallery.LastModifiedBy = _signedUser;

            _context.SaveChanges();

            var viewModel = new GalleryViewModel
            {
                Gallery = _context.Galleries
                    .Include(g => g.Pictures)
                    .SingleOrDefault(g => g.Id == picture.GalleryId)
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePicture(Guid id)
        {
            var picToDelete = _context.Pictures.SingleOrDefault(p => p.Id == id);
            var dbGallery = _context.Galleries.SingleOrDefault(g => g.Id == picToDelete.GalleryId);

            if (picToDelete == null || dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            _context.Pictures.Remove(picToDelete);

            dbGallery.LastModification = DateTime.Now;
            dbGallery.LastModifiedBy = _signedUser;

            _context.SaveChanges();

            var dirPath = Server.MapPath(string.Format("~/Pictures/{0}", picToDelete.GalleryId));
            var serverPath = Path.Combine(dirPath + "\\", picToDelete.FileName);

            if (System.IO.File.Exists(serverPath))
                System.IO.File.Delete(serverPath);

            var viewModel = new GalleryViewModel
            {
                Gallery = _context.Galleries
                    .Include(g => g.Pictures)
                    .SingleOrDefault(g => g.Id == picToDelete.GalleryId)
            };

            return View($"Edit", viewModel);
        }

    }
}