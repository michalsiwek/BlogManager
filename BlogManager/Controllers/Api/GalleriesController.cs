using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Http;

namespace BlogManager.Controllers.Api
{
    public class GalleriesController : ApiController
    {
        private ApplicationDbContext _context;

        public GalleriesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IHttpActionResult GetGalleries()
        {
            var galleries = _context.Galleries
                .Include(g => g.Account)
                .Include(g => g.Pictures)
                .Where(g => g.IsVisible)
                .ToList()
                .Select(Mapper.Map<Gallery, GalleryDto>);

            return Ok(galleries);
        }

        [HttpGet]
        public IHttpActionResult GetGallery(int id)
        {
            var gallery = _context.Galleries
                .Include(g => g.Account)
                .Include(g => g.Pictures)
                .SingleOrDefault(g => g.Id == id && g.IsVisible == true);

            if (gallery == null)
                return NotFound();

            return Ok(Mapper.Map<Gallery, GalleryDto>(gallery));
        }

        /*[HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteGallery(int id)
        {
            var galleryFromDb = _context.Galleries.SingleOrDefault(g => g.Id == id);

            if (galleryFromDb == null)
                return NotFound();

            _context.Galleries.Remove(galleryFromDb);
            _context.SaveChanges();

            return Ok();
        }*/
    }
}