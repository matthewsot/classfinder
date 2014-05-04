using Classfinder.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Classfinder.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        [ValidateInput(false), AllowAnonymous]
        public ActionResult Index(string username, string device)
        {
            if (username != null)
            {
                using (CfDb db = new CfDb())
                {
                    if (db.Users.Count(a => a.Username == username) > 0)
                    {
                        if((Request.Browser.IsMobileDevice || device.ToLower().StartsWith("m")) && !device.ToLower().StartsWith("d"))
                        {
                            ViewBag.Mobile = true;
                        }
                        else
                        {
                            ViewBag.Mobile = false;
                        }
                        ViewBag.Username = username;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", controllerName: "Default");
        }

    }
}
