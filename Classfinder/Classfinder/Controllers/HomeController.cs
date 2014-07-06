using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //TODO: fix this
            return RedirectToRoute("Schedule/" + HttpUtility.UrlEncode(User.Identity.GetUserName()));
        }

        [HttpGet]
        [Route("Schedule/{userName}")]
        [AllowAnonymous]
        public ActionResult Schedule(string userName)
        {
            using (var db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(usr => usr.UserName == userName);

                ViewBag.FirstSemester = user.FirstSemester;
                ViewBag.SecondSemester = user.SecondSemester;

                return View();
            }
        }
    }
}