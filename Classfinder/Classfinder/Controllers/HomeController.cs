using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CfDb db = new CfDb();

        private ICollection<Class> CheckAndFillWithNoClass(ICollection<Class> schedule, string school)
        {
            for (var i = 1; i <= 7; i++)
            {
                if (schedule.All(@class => @class.Period != i))
                {
                    var noClass =
                        db.Classes.FirstOrDefault(
                            @class => @class.Name == "No Class" && @class.Period == i && @class.School == school);
                    
                    if (noClass == null)
                    {
                        noClass = new Class()
                        {
                            Name = "No Class",
                            Period = i,
                            School = school
                        };
                        db.Classes.Add(noClass);
                    }

                    schedule.Add(noClass);
                }
            }
            db.SaveChanges();
            return schedule;
        }

        // GET: Home
        public ActionResult Index()
        {
            //If users change their username, sometimes User.Identity.GetUserName() doesn't always return the latest username
            var user = db.Users.Find(User.Identity.GetUserId());
            return Redirect("/Schedule/" + user.UserName);
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

            ViewBag.RealName = user.RealName;
            ViewBag.ViewingUserName = user.UserName;
            ViewBag.UserId = user.Id;

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = db.Users.Find(User.Identity.GetUserId());
                ViewBag.UserName = loggedInUser.UserName;
            }

            ViewBag.FirstSemester = CheckAndFillWithNoClass(user.FirstSemester, user.School).OrderBy(@class => @class.Period);
            //ViewBag.SecondSemester = CheckAndFillWithNoClass(user.SecondSemester, user.School).OrderBy(@class => @class.Period);

            ViewBag.Description = "";
            foreach (var @class in user.FirstSemester)
            {
                if (@class.Name == "No Class") continue;
                var teacher = @class.Name.Contains(",") ? @class.Name.Substring(0, @class.Name.LastIndexOf(',')) : "";
                if (teacher.Trim().Length > 0)
                {
                    ViewBag.Description += @class.Period + ": " + @class.Name.Split(',').Last().Trim() + " - " + teacher + ", ";
                }
                else
                {
                    ViewBag.Description += @class.Period + ": " + @class.Name.Split(',').Last().Trim() + ", ";
                }
            }

            if (ViewBag.Description.Length > 2)
            {
                ViewBag.Description = ViewBag.Description.Substring(0, ViewBag.Description.Length - 2).Trim();
            }

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