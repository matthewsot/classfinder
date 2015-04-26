using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classfinder.Models
{
    public class SignUpModel
    {
        public string Password { get; set; }
        public int GradYear { get; set; }
        public string School { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class ResetPassModel
    {
        public string Username { get; set; }
    }
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangeUsernameModel
    {
        public string NewUsername { get; set; }
    }
    public class ChangeNameModel
    {
        public string NewName { get; set; }
    }

    public class ResetPasswordModel
    {
        public string token { get; set; }
        public string userId { get; set; }
        public string newPassword { get; set; }
    }
}