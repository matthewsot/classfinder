using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Classfinder.App_Start
{
    public class AuthFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //via http://weblogs.asp.net/jgalloway/archive/2012/04/18/asp-net-mvc-authentication-global-authentication-and-allow-anonymous.aspx
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                var x = WebSecurity.IsAuthenticated;
                if (!WebSecurity.IsAuthenticated)
                {
                    RedirectToRoute(filterContext, new { controller = "Default", action = "Index" });
                }
            }
        }
        //via http://forums.asp.net/p/1239842/2262373.aspx
        private void RedirectToRoute(AuthorizationContext context, object routeValues)
        {
            var rc = new RequestContext(context.HttpContext, context.RouteData);
            string url = RouteTable.Routes.GetVirtualPath(rc,
                new RouteValueDictionary(routeValues)).VirtualPath;
            context.HttpContext.Response.Redirect(url, true);
        }
    }
}