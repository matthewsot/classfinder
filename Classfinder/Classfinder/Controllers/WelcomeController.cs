using System.Web.Mvc;

namespace Classfinder.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Schedule(int? id)
        {
            var semester = id;
            if (!semester.HasValue || semester > 2 || semester < 1)
            {
                semester = 1;
            }

            ViewBag.StepNum = semester + 1;

            return View();
        }
    }
}