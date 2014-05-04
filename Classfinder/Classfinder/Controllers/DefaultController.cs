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
                var Username = WebSecurity.CurrentUserName;
                if (Roles.IsUserInRole(Username, "Student"))
                {
                    ViewBag.Redirect = "/Schedule/" + WebSecurity.CurrentUserName;
                    return View("Redirecter");
                }
                else if (Roles.IsUserInRole(Username, "Teacher"))
                {
                    ViewBag.Redirect = "/Teacher";
                    return View("Teacher");
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
