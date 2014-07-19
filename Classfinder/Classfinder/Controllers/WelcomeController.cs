using System.Linq;
using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        CfDb db = new CfDb();

        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                
            }
            return View();
        }

        public ActionResult Schedule(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.School = user.School;

            var semester = id;
            if (!semester.HasValue || semester > 2 || semester < 1)
            {
                semester = ((SignUpLevel)user.SignUpLevel == SignUpLevel.FirstSemesterScheduleEntered) ? 2 : 1;
            }

            if (semester == 2 && user.SignUpLevel < (int) SignUpLevel.SecondSemesterScheduleEntered)
            {
                user.SignUpLevel = (int) SignUpLevel.SecondSemesterScheduleEntered;
            }

            //if (semester == 2 && !user.SecondSemester.Any())
            //{
            //    foreach (var @class in user.FirstSemester)
            //    {
            //        user.SecondSemester.Add(@class);
            //    }
            //}

            db.SaveChanges();

            ViewBag.StepNum = semester + 1;

            return View();
        }

        public ActionResult Complete()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.SignUpLevel = (int) SignUpLevel.SecondSemesterScheduleEntered;
            db.SaveChanges();

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