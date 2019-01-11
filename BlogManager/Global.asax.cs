using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Categories;
using BlogManager.Models.Entries;
using BlogManager.Models.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BlogManager
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entry, EntryDto>()
                .ForMember(e => e.Author, a => a.MapFrom(b => b.Account.Nickname))
                .ForMember(e => e.LastEditor, a => a.MapFrom(b => b.LastModifiedBy.Nickname))
                .ForMember(e => e.Content, a => a.MapFrom(b => b.Paragraphs.Select(p => p.Body)))
                .ForMember(e => e.CategoryId, c => c.MapFrom(b => b.ContentCategory.Id))
                .ForMember(e => e.CategoryName, c => c.MapFrom(b => b.ContentCategory.Name))
                .ForMember(e => e.SubcategoryId, c => c.MapFrom(b => b.ContentSubcategory.Id))
                .ForMember(e => e.SubcategoryName, c => c.MapFrom(b => b.ContentSubcategory.Name));

                cfg.CreateMap<Gallery, GalleryDto>()
                .ForMember(g => g.CreatedBy, a => a.MapFrom(b => b.Account.Nickname))
                .ForMember(g => g.LastModifiedBy, a => a.MapFrom(b => b.LastModifiedBy.Nickname))
                .ForMember(g => g.Pictures, a => a.MapFrom(b => b.Pictures))
                .ForMember(e => e.CategoryId, c => c.MapFrom(b => b.ContentCategory.Id))
                .ForMember(e => e.CategoryName, c => c.MapFrom(b => b.ContentCategory.Name))
                .ForMember(e => e.SubcategoryId, c => c.MapFrom(b => b.ContentSubcategory.Id))
                .ForMember(e => e.SubcategoryName, c => c.MapFrom(b => b.ContentSubcategory.Name));

                cfg.CreateMap<Picture, PictureDto>();

                cfg.CreateMap<ContentCategory, ContentCategoryDto>();

                cfg.CreateMap<ContentSubcategory, ContentSubcategoryDto>();
            });

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
