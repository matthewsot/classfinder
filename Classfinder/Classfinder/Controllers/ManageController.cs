using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        CfDb db = new CfDb();

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

        public ActionResult Password()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
                db = null;
            }
            base.Dispose(disposing);
        }
    }
}