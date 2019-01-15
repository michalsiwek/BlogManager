using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BlogManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /* Custom */

            config.Routes.MapHttpRoute(
                name: "API Cat",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                name: "API SubCat Entries",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "ContentSubcategories", action = "Entries", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "API SubCat Galleries",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "ContentSubcategories", action = "Galleries", id = RouteParameter.Optional }
            );
        }
    }
}
