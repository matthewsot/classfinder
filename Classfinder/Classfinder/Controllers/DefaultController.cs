using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Classfinder.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        //
        // GET: /Default/
        public ActionResult Index(string device)
        {
            if (WebSecurity.IsAuthenticated)
            {
                var username = WebSecurity.CurrentUserName;
                if (Roles.IsUserInRole(username, "Student"))
                {
                    ViewBag.Redirect = "/Schedule/" + WebSecurity.CurrentUserName;
                    return View("Redirecter");
                }
                
                if (Roles.IsUserInRole(username, "Teacher"))
                {
                    //ViewBag.Redirect = "/Teacher";
                    //return View("Teacher");
                }
            }
            if ((Request.Browser.IsMobileDevice || device.ToLower().StartsWith("m")) && !device.ToLower().StartsWith("d"))
            {
                ViewBag.Mobile = true;
            }
            else
            {
                ViewBag.Mobile = false;
            }
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
