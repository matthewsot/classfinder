using System.Web.Mvc;

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
            return View();
        }
    }
}