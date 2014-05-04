using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classfinder.Database;
using WebMatrix.WebData;

namespace Classfinder.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string Username, string Password, bool Remember = false, bool Silent = false, string Redirect = "/Schedule/[username]")
        {
            Redirect = Redirect.Replace("[username]", Username);
            bool isGood = WebSecurity.Login(Username, Password, Remember);
            if (isGood)
            {
                if (!Silent)
                {
                    ViewBag.Redirect = Redirect;
                    return View("Redirecter");
                }
                else
                {
                    ViewBag.Raw = Redirect;
                    return View("Blank");
                }
            }
            else
            {
                ViewBag.Raw = "FALSE";
                return View("Blank");
            }
        }

        [HttpGet]
        public ActionResult Logout(bool Silent = false, string Redirect = "/")
        {
            WebSecurity.Logout();
            if (!Silent)
            {
                ViewBag.Redirect = Redirect;
                return View("Redirecter");
            }
            else
            {
                ViewBag.Raw = Redirect;
                return View("Blank");
            }
        }
        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Classes()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPass(string code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}