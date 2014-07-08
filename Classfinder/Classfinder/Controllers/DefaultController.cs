using System.Web.Mvc;

namespace Classfinder.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ShowLogin = ViewBag.ShowLogin ?? false;
            return View();
        }

        [Route("LogIn")]
        public ActionResult LogIn()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ShowLogin = true;
                return View("Index");
            }
        }
    }
}