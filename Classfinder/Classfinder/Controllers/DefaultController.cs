using System.Web;
using System.Web.Mvc;

namespace Classfinder.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowLogin = ViewBag.ShowLogin ?? false;
            return View();
        }

        [Route("LogIn")]
        public ActionResult LogIn(string returnUrl)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = HttpUtility.UrlEncode(returnUrl ?? "");
            ViewBag.ShowLogin = true;
            return View("Index");
        }
    }
}