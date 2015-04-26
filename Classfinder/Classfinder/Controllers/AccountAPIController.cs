using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Classfinder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Classfinder.Controllers
{
    [Authorize]
    public class AccountAPIController : ApiController
    {
        private CfDb db = new CfDb();

        [HttpPost]
        [Route("API/Account/SignUp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignUp(SignUpModel model)
        {
            using (var userManager = new UserManager<UserAccount>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(db)))
            {
                var errors = new List<string>();

                // TODO: Should be validating with ModelState
                if (model.Password.Length <= 6) errors.Add("Password");
                if (!(model.GradYear <= 2018 && model.GradYear >= 2015)) errors.Add("GradYear");
                if (model.FullName != null && model.FullName.Length > 50) errors.Add("FullName");
                if (db.Users.Any(usr => usr.UserName == model.Username)) errors.Add("Username");

                if (errors.Count > 0)
                {
                    return Ok(string.Join(",", errors) + ",");
                }

                var user = new UserAccount { UserName = model.Username, RealName = model.FullName, Email = model.Email, GradYear = model.GradYear, SignUpLevel = ((int)SignUpLevel.Registered), School = model.School };

                var result = await userManager.CreateAsync(user, model.Password);
                return Ok(result.Succeeded ? "GOOD" : string.Join(",", errors));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("API/Account/ResetPassword/")]
        public async Task<IHttpActionResult> ResetPassword(ResetPassModel model)
        {
            using (var userManager = new UserManager<UserAccount>(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(db)))
            {
                //Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
                if (Startup.DataProtectionProvider != null)
                {
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<UserAccount>(Startup.DataProtectionProvider.Create("PasswordReset"));
                }

                var user = db.Users.FirstOrDefault(u => u.UserName == model.Username);

                if (user == null) return Ok("NO USER");
                if (user.Email == null) return Ok("NO EMAIL");

                //Thanks! http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
                var mail = new MailMessage();
                var smtpServer = new SmtpClient();

                mail.From = new MailAddress("noreply@classfinder.me", "Classfinder");
                mail.To.Add(new MailAddress(user.Email, user.RealName));
                mail.Subject = "Reset Your Password";
                mail.Body = "Please visit http://classfinder.me/ResetPass?token=" +
                            HttpUtility.UrlEncode(await userManager.GeneratePasswordResetTokenAsync(user.Id)) +
                            "&userId=" + HttpUtility.UrlEncode(user.Id) + " to reset your Classfinder password.";

                smtpServer.Send(mail);
                return Ok("GOOD");
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("API/Account/DoResetPassword/")]
        //public async Task<IHttpActionResult> DoResetPassword(ResetPasswordModel model)
        //{
        //    using (var userManager = new UserManager<UserAccount>(
        //        new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(db)))
        //    {
        //        //Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
        //        if (Startup.DataProtectionProvider != null)
        //        {
        //            userManager.UserTokenProvider = new DataProtectorTokenProvider<UserAccount>(Startup.DataProtectionProvider.Create("PasswordReset"));
        //        }
        //    }
        //}

        [HttpPost]
        [Route("API/Account/ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            using (var userManager = new UserManager<UserAccount>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(db)))
            {
                var result = await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("GOOD");
                }
                else
                {
                    return Ok("ERROR");
                }
            }
        }

        [HttpPost]
        [Route("API/Account/ChangeUsername")]
        public IHttpActionResult ChangeUsername(ChangeUsernameModel model)
        {
            //todo: they don't all use the db, there's no need to init it for everything
            model.NewUsername = model.NewUsername.Trim();

            var user = db.Users.Find(User.Identity.GetUserId());

            if (db.Users.Any(usr => usr.UserName == model.NewUsername))
            {
                return Ok("EXISTING");
            }
            user.UserName = model.NewUsername;
            db.SaveChanges();
            return Ok("GOOD");
        }

        [HttpPost]
        [Route("API/Account/ChangeName")]
        public IHttpActionResult ChangeName(ChangeNameModel model)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (string.IsNullOrWhiteSpace(model.NewName)) return Ok("WHITESPACE");

            user.RealName = model.NewName;
            db.SaveChanges();

            return Ok("GOOD");
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
