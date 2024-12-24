using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KariyerPortali5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Home", action = "Dashboard", id = UrlParameter.Optional }
);

            routes.MapRoute(
                name: "Ilanlar",
                url: "ilanlar",
                defaults: new { controller = "Ilan", action = "Index" }
             );

            routes.MapRoute(
    name: "SirketLogin",
    url: "SirketLogin/{action}/{id}",
    defaults: new { controller = "SirketLogin", action = "Index", id = UrlParameter.Optional }
);
            routes.MapRoute(
    name: "SirketHome",
    url: "SirketHome/{action}/{id}",
    defaults: new { controller = "SirketHome", action = "Dashboard", id = UrlParameter.Optional }
);
            routes.MapRoute(
    name: "SirketIlan",
    url: "SirketIlan/{action}/{id}",
    defaults: new { controller = "SirketIlan", action = "Create", id = UrlParameter.Optional }
);
            routes.MapRoute(
    name: "Ilanlarim",
    url: "Ilanlarim/{action}/{id}",
    defaults: new { controller = "Ilanlarim", action = "Index", id = UrlParameter.Optional }
);
            routes.MapRoute(
    name: "Basvurularim",
    url: "Basvurularim",
    defaults: new { controller = "Basvurularim", action = "Index" }
);


        }
    }
}