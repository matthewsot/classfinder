using System.Web.Mvc;

namespace Classfinder.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("LogIn")]
        public ActionResult LogIn()
        {
            return View();
        }
    }
}