using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Classfinder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Classfinder.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowLogin = ViewBag.ShowLogin ?? false;
            return View();
        }

        [Route("LogIn")]
        public ActionResult LogIn(string returnUrl)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = HttpUtility.UrlEncode(returnUrl ?? "");
            ViewBag.ShowLogin = true;
            return View("Index");
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            var db = new CfDb();
            using (var userManager = new UserManager<UserAccount>(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(new CfDb())))
            {
                //Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
                if (Startup.DataProtectionProvider != null)
                {
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<UserAccount>(Startup.DataProtectionProvider.Create("PasswordReset"));
                }

                await userManager.ResetPasswordAsync(model.userId, model.token, model.newPassword);
                ViewBag.ShowLogin = true;
                return View("Index");
            }
        }

        [Route("ResetPass")]
        public ActionResult ResetPass(string token, string userId)
        {
            ViewBag.Token = token;
            ViewBag.UserID = userId;
            return View();
        }
    }
}