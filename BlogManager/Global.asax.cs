using AutoMapper;
using BlogManager.Dtos;
using BlogManager.Models.Entries;
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
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Entry, EntryDto>()
                .ForMember(e => e.Author, a => a.MapFrom(b => b.Account.UserName))
                .ForMember(e => e.LastEditor, a => a.MapFrom(b => b.LastModifiedBy.UserName))
                .ForMember(e => e.Content, a => a.MapFrom(b => b.Paragraphs.Select(p => p.Body)))
                .ForMember(e => e.CategoryId, c => c.MapFrom(b => b.EntryCategory.Id))
                .ForMember(e => e.CategoryName, c => c.MapFrom(b => b.EntryCategory.Name)));
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
