﻿using System;
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
                name: "Entry",
                url: "{controller}/{action}",
                defaults: new { controller = "Entry", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Entries",
                url: "{controller}/{action}",
                defaults: new { controller = "Entries", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EntryCategory",
                url: "{controller}/{action}",
                defaults: new { controller = "EntryCategory", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EntryCategories",
                url: "{controller}/{action}",
                defaults: new { controller = "EntryCategories", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
