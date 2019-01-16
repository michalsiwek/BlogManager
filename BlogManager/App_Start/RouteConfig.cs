using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account",
                url: "{controller}/{action}",
                defaults: new { controller = "Entry", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Accounts",
                url: "{controller}/{action}",
                defaults: new { controller = "Accounts", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Preview",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Entry", action = "Preview", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Entries",
                url: "{controller}/{action}",
                defaults: new { controller = "Entries", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ContentCategories",
                url: "{controller}/{action}",
                defaults: new { controller = "ContentCategories", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Galleries",
                url: "{controller}/{action}",
                defaults: new { controller = "Galleries", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
