using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Classfinder.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Roles.IsUserInRole("Teacher"))
            {
                return RedirectToAction("Teacher");
            }
            else if (Roles.IsUserInRole("Administrator"))
            {
                return RedirectToAction("School");
            }
            else if (Roles.IsUserInRole("SiteAdmin"))
            {
                return RedirectToAction("Site");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult School()
        {
            return View();
        }

        public ActionResult Teacher()
        {
            return View();
        }

        public ActionResult Site()
        {
            return View();
        }
    }
}
