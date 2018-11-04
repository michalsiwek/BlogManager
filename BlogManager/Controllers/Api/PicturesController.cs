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
    public class PicturesController : ApiController
    {
        private ApplicationDbContext _context;

        public PicturesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IHttpActionResult GetPicture(Guid id)
        {
            var picture = _context.Pictures
                .SingleOrDefault(g => g.Id == id);

            if (picture == null)
                return NotFound();

            var gallery = _context.Galleries.SingleOrDefault(g => g.Id == picture.GalleryId);

            if (!gallery.IsVisible)
                return NotFound();

            return Ok(Mapper.Map<Picture, PictureDto>(picture));
        }
    }
}