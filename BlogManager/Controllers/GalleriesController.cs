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
using BlogManager.Repositories;

namespace BlogManager.Controllers
{
    public class GalleriesController : Controller
    {
        private ApplicationDbContext _context;
        private DbRepository _dbRepository = new DbRepository();

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

        public ActionResult Edit(int id)
        {
            var dbGallery = _context.Galleries
                .Include(g => g.Pictures)
                .Include(g => g.Account)
                .SingleOrDefault(g => g.Id == id);

            if (dbGallery == null)
                return HttpNotFound();

            var viewModel = new GalleryViewModel
            {
                Gallery = dbGallery
            };

            viewModel.Gallery.Pictures = _context.Pictures.Where(p => p.GalleryId == id).ToList();

            return View(viewModel);
        }

        [HttpPost]
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
                dbGallery.Title = gallery.Title;
                dbGallery.Description = gallery.Description;
                dbGallery.IsVisible = gallery.IsVisible;

                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        var dirPath = Server.MapPath(string.Format("~/Pictures/{0}", dbGallery.Id));
                        var serverPath = Path.Combine(dirPath + "\\", fileName);
                        var dbPath = string.Format("/Pictures/{0}/{1}", dbGallery.Id, fileName);
                        file.SaveAs(serverPath);
                        gallery.Pictures.Add(new Picture(file.FileName, dbPath, DateTime.Now, dbGallery.Id));
                    }

                    foreach (var p in gallery.Pictures)
                        _context.Pictures.Add(p);
                }
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