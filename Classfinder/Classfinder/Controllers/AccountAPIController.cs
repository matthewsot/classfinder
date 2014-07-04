using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Classfinder.Models;
using Microsoft.AspNet.Identity;

namespace Classfinder.Controllers
{
    public class SignUpModel
    {
        public string Password { get; set; }
        public int Year { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
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
                if (!(model.Year <= 2017 && model.Year >= 2015)) errors.Add("Year");
                if (model.FullName != null && model.FullName.Length > 50) errors.Add("FullName");
                if (db.Users.Count(usr => usr.UserName == model.Username) > 0) errors.Add("Username");

                if (errors.Count > 0)
                {
                    return Ok(string.Join(",", errors) + ",");
                }

                var user = new UserAccount { UserName = model.Username, Email = model.Email, GradYear = model.Year, SignUpLevel = ((int)SignUpLevel.Registered) };

                var result = await userManager.CreateAsync(user, model.Password);
                return Ok(result.Succeeded ? "GOOD" : string.Join(",", errors));
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
