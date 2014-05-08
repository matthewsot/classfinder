using Classfinder.Database;
using System.Linq;
using System.Web.Mvc;

namespace Classfinder.Controllers
{
    public class HomeController : Controller
    {
        [ValidateInput(false), AllowAnonymous]
        public ActionResult Index(string username, string device)
        {
            if (username == null) return RedirectToAction("Index", controllerName: "Default");
            using (var db = new CfDb())
            {
                if (db.Users.Count(a => a.Username == username) <= 0)
                {
                    return RedirectToAction("Index", controllerName: "Default");
                }

                if((Request.Browser.IsMobileDevice || device.ToLower().StartsWith("m")) && !device.ToLower().StartsWith("d"))
                {
                    ViewBag.Mobile = true;
                }
                else
                {
                    ViewBag.Mobile = false;
                }
                ViewBag.Username = username;
                return View();
            }
        }

    }
}
