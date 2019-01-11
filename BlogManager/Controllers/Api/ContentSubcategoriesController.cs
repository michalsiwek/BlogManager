using BlogManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogManager.Infrastructure;
using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Categories;
using BlogManager.Repositories.Api;

namespace BlogManager.Controllers.Api
{
    public class ContentSubcategoriesController : ApiController
    {
        private ICatetegoriesRepository _repo;

        public ContentSubcategoriesController()
        {
            _repo = new CategoriesRepository();
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