using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogManager.Infrastructure;
using BlogManager.Repositories;
using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Categories;

namespace BlogManager.Controllers.Api
{
    public class ContentCategoriesController : ApiController
    {
        private IContentCategoryRepository _repo;

        public ContentCategoriesController()
        {
            _repo = new ContentCategoryRepository(new ContentCategoryManageService());
        }

        [HttpGet]
        public IHttpActionResult GetContentCategories()
        {
            var contentCategories = _repo.GetActiveContentCategories();

            if (contentCategories == null)
                return NotFound();

            return Ok(contentCategories.ToList().Select(Mapper.Map<ContentCategory, ContentCategoryDto>));
        }

    }
}