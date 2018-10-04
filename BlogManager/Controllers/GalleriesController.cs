﻿using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BlogManager.Controllers
{
    public class GalleriesController : Controller
    {
        private ApplicationDbContext _context;

        public GalleriesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new GalleriesViewModel
            {
                Galleries = _context.Galleries.Include(e => e.Account).ToList()
            };

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

        [HttpPost]
        public ActionResult Save(Gallery gallery)
        {
            var dbGallery = _context.Galleries
                .SingleOrDefault(e => e.Id == gallery.Id);

            if (dbGallery == null)
            {
                gallery.CreateDate = DateTime.Now;
                gallery.Account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
                gallery.Pictures = new List<Picture>();

                _context.Galleries.Add(gallery);

                foreach (var p in gallery.Pictures)
                    _context.Pictures.Add(p);
            }
            else
            {

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Galleries");
        }

        [HttpPost]
        public ActionResult Validate(int galleryId, string isVisible)
        {
            var galleryToValidate = _context.Galleries.SingleOrDefault(e => e.Id == galleryId);
            if (galleryToValidate == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if(account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

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
        public ActionResult Delete(int galleryId)
        {
            var galleryToDelete = _context.Galleries.SingleOrDefault(e => e.Id == galleryId);
            if (galleryToDelete == null)
                return HttpNotFound();

            var account = _context.Users.SingleOrDefault(u => u.Email.Equals(User.Identity.Name));
            /*if (account == null)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "You have no permission to perform this action");*/

            _context.Galleries.Remove(galleryToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Galleries");
        }
    }
}