using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Classfinder
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Schedule",
                url: "Schedule/{username}/{device}",
                defaults: new { controller = "Home", action = "Index", username = UrlParameter.Optional, device = "none" }
            );

            //OK, this is a little messy, I'll fix it sometime...
            routes.MapRoute(
                name: "Loginer",
                url: "mobile",
                defaults: new { controller = "Default", action = "Index", device = "mobile" });

            routes.MapRoute(
                name: "LoginDesktop",
                url: "desktop",
                defaults: new { controller = "Default", action = "Index", device = "desktop" });

            routes.MapRoute(
                name: "FAQ",
                url: "FAQ",
                defaults: new { controller = "Default", action = "FAQ" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{code}/{device}",
                defaults: new { controller = "Default", action = "Index", code = UrlParameter.Optional, device = "none" }
            );
            //routes.MapRoute(
            //    name: "Schedule",
            //    url: "Schedule/{username}/{device}",
            //    defaults: new { controller = "Home", action = "Index", username = UrlParameter.Optional, device = "none" }
            //);

            //routes.MapRoute(
            //    name: "FAQ",
            //    url: "FAQ",
            //    defaults: new { controller = "Default", action = "FAQ" });

            ////////OK, this is a little messy, I'll fix it sometime...
            //////routes.MapRoute(
            //////    name: "Loginer",
            //////    url: "mobile",
            //////    defaults: new { controller = "Default", action = "Index", device = "mobile" });

            //////routes.MapRoute(
            //////    name: "LoginDesktop",
            //////    url: "desktop",
            //////    defaults: new { controller = "Default", action = "Index", device = "mobile" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{code}/{device}",
            //    defaults: new { controller = "Default", action = "Index", code = UrlParameter.Optional }
            //);
        }
    }
}