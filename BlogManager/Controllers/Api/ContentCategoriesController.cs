using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogManager.Infrastructure;
using BlogManager.Repositories.Api;
using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Categories;

namespace BlogManager.Controllers.Api
{
    public class ContentCategoriesController : ApiController
    {
        private ICatetegoriesRepository _repo;

        public ContentCategoriesController()
        {
            _repo = new CategoriesRepository();
        }

        [Route("api/ContentCategories/Entries")]
        [HttpGet]
        public IHttpActionResult Entries()
        {
            var contentCategories = _repo.GetActiveEntryContentCategories();

            if (contentCategories == null)
                return NotFound();

            return Ok(contentCategories.ToList().Select(Mapper.Map<ContentCategory, ContentCategoryDto>));
        }

        [Route("api/ContentCategories/Galleries")]
        [HttpGet]
        public IHttpActionResult Galleries()
        {
            var contentCategories = _repo.GetActiveGalleryContentCategories();

            if (contentCategories == null)
                return NotFound();

            return Ok(contentCategories.ToList().Select(Mapper.Map<ContentCategory, ContentCategoryDto>));
        }

    }
}