using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Schedule()
        {
            using (var db = new CfDb())
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.School = user.School;
                return View();
            }
        }
    }
}