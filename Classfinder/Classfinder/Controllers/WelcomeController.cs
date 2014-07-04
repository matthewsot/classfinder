﻿using System.Web.Mvc;
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
            return View();
        }

        public ActionResult Schedule(int? id)
        {
            var semester = id;
            if (!semester.HasValue || semester > 2 || semester < 1)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                semester = ((SignUpLevel)user.SignUpLevel == SignUpLevel.FirstSemesterScheduleEntered) ? 2 : 1;
            }

            ViewBag.StepNum = semester + 1;

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