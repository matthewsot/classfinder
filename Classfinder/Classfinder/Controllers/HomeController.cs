using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private CfDb db = new CfDb();

        private ICollection<Class> CheckAndFillWithNoClass(ICollection<Class> schedule)
        {
            for (var i = 1; i <= 7; i++)
            {
                if (!schedule.Any(@class => @class.Period == i))
                {
                    schedule.Add(db.Classes.First(@class => @class.Name == "No Class" && @class.Period == i));
                }
            }
            db.SaveChanges();
            return schedule;
        }

        // GET: Home
        public ActionResult Index()
        {
            return Redirect("/Schedule/" + User.Identity.GetUserName());
        }

        [HttpGet]
        [Route("Schedule/{userName}")]
        [AllowAnonymous]
        public ActionResult Schedule(string userName)
        {
            var user = db.Users.FirstOrDefault(usr => usr.UserName == userName);

            if (user == null)
            {
                //TODO: handle user-not-found errors
            }

            ViewBag.UserName = user.RealName;

            ViewBag.FirstSemester = CheckAndFillWithNoClass(user.FirstSemester).OrderBy(@class => @class.Period);
            ViewBag.SecondSemester = CheckAndFillWithNoClass(user.SecondSemester).OrderBy(@class => @class.Period);

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}