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
    public class ContentSubcategoriesController : ApiController
    {
        private IContentCategoryRepository _repo;

        public ContentSubcategoriesController()
        {
            _repo = new ContentCategoryRepository(new ContentCategoryManageService());
        }

        [HttpGet]
        public IHttpActionResult GetContentSubcategories(int id)
        {
            var contentSubcategories = _repo.GetContentSubcategoriesByParentId(id);

            if (contentSubcategories == null)
                return NotFound();

            return Ok(contentSubcategories.ToList().Select(Mapper.Map<ContentSubcategory, ContentSubcategoryDto>));
        }
    }
}