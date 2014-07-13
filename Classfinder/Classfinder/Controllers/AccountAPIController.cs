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
    public class SignUpModel
    {
        public string Password { get; set; }
        public int GradYear { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class ResetPassModel
    {
        public string Email { get; set; }
    }

    [Authorize]
    public class AccountAPIController : ApiController
    {
        private CfDb db = new CfDb();

        //modified from the lhs-campaign source ;) https://github.com/matthewsot/lhs-campaign file /Controllers/AccountsAPIController.cs
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
                if (db.Users.Count(usr => usr.UserName == model.Username) > 0) errors.Add("Username");

                if (errors.Count > 0)
                {
                    return Ok(string.Join(",", errors) + ",");
                }

                var user = new UserAccount { UserName = model.Username, RealName = model.FullName, Email = model.Email, GradYear = model.GradYear, SignUpLevel = ((int)SignUpLevel.Registered), School = "Lynbrook" };

                var result = await userManager.CreateAsync(user, model.Password);
                return Ok(result.Succeeded ? "GOOD" : string.Join(",", errors));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("API/Account/ResetPassword/{username}")]
        public async Task<IHttpActionResult> ResetPassword(string username)
        {
            using (var userManager = new UserManager<UserAccount>(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<UserAccount>(db)))
            {
                //Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
                if (Startup.DataProtectionProvider != null)
                {
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<UserAccount>(Startup.DataProtectionProvider.Create("PasswordReset"));
                }

                var user = db.Users.FirstOrDefault(u => u.UserName == username);

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
                            " to reset your Classfinder password.";

                smtpServer.Send(mail);
                return Ok("GOOD");
            }
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
