using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using BlogManager.Helpers;
using BlogManager.Helpers.Enums;
using BlogManager.Infrastructure;
using BlogManager.Models.Accounts;
using BlogManager.Repositories;

namespace BlogManager.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IGalleryRepository _galleryRepo;
        private readonly IFileUploadService _fileUploadService;

        private Account _signedUser;

        public GalleriesController()
        {
            _accountRepo = new AccountRepository(new AccountManageService(), new MailingService());
            _fileUploadService = new FileUploadService(new FileSecurityValidator());
            _galleryRepo = new GalleryRepository(new FileSecurityValidator(), _fileUploadService);         
            _signedUser = new Account();
        }

        private void GetSignedUser()
        {
            _signedUser = _accountRepo.GetAccountByEmail(User.Identity.Name);
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new GalleriesViewModel
            {
                Galleries = _galleryRepo.GetGalleries()
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

            var dbGallery = _galleryRepo.GetGalleryById(id);

            if (dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            var viewModel = new GalleryViewModel(dbGallery);

            viewModel.Gallery.Pictures = _galleryRepo.GetGalleryPictures(id);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Gallery gallery)
        {
            var result = _galleryRepo.SaveGalleryChanges(gallery, User.Identity.Name, Request, Server);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return RedirectToAction("Index", "Home");
            
            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int galleryId, string isVisible)
        {
            GetSignedUser();
            if (!_signedUser.CanManageAllContent())
                return RedirectToAction("Index", "Home");

            var result = _galleryRepo.ValidateGallery(galleryId, isVisible);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int galleryId)
        {
            var result = _galleryRepo.DeleteGallery(galleryId, User.Identity.Name);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return RedirectToAction("Index", "Home");
            
            _fileUploadService.DeleteDirectory(Server, galleryId.ToString());

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPictures(Gallery gallery)
        {
            ModelState.Clear();

            var result = _galleryRepo.SaveNewPictures(gallery, User.Identity.Name, Request, Server);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (result == DbRepoStatusCode.PartialSuccess)
                ModelState.AddModelError("", $"One or more files upload failed due to filename conflict");

            var viewModel = new GalleryViewModel
            {
                Gallery = _galleryRepo.GetGalleryById(gallery.Id)
            };

            return View("Edit", viewModel);
        }

        public ActionResult PictureEdit(Guid id)
        {
            var dbPicture = _galleryRepo.GetPictureById(id);
            var dbGallery = _galleryRepo.GetGalleryById(dbPicture.GalleryId);

            if (dbPicture == null || dbGallery == null)
                return HttpNotFound();

            GetSignedUser();
            if (!_signedUser.CanManageAllContent() && !dbGallery.Account.Equals(_signedUser))
                return RedirectToAction("Index", "Home");

            var viewModel = new PictureViewModel
            {
                Picture = dbPicture
            };
            
            return PartialView("_PictureEdit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PictureSaveChanges(Picture picture)
        {
            var result = _galleryRepo.SavePictureChanges(picture, User.Identity.Name);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return RedirectToAction("Index", "Home");

            var viewModel = new GalleryViewModel
            {
                Gallery = _galleryRepo.GetGalleryById(picture.GalleryId)
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePicture(Guid id)
        {
            var picToDelete = _galleryRepo.GetPictureById(id);

            if (picToDelete == null)
                return HttpNotFound();

            var result = _galleryRepo.DeletePicture(id, User.Identity.Name, Server);

            if (result == DbRepoStatusCode.NotFound)
                return HttpNotFound();

            if (result == DbRepoStatusCode.BadRequest)
                return RedirectToAction("Index", "Home");

            _fileUploadService.DeleteFile(Server, picToDelete.GalleryId.ToString(), picToDelete.FileName);

            var viewModel = new GalleryViewModel
            {
                Gallery = _galleryRepo.GetGalleryById(picToDelete.GalleryId)
            };

            return View($"Edit", viewModel);
        }

    }
}