﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Faktura
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Order", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Second",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Invoice", id = UrlParameter.Optional }
            );
        }
    }
}
