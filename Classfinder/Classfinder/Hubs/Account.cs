using Classfinder.Database;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using System.Net.Mail;

namespace Classfinder.Hubs
{
    //TODO: why the heck is this using SignalR. Rewrite this with WebAPI
    public class Account : Hub
    {
        public int Signup(string Username, string Password, string Realname, string Email, string SchoolCode, int Grade, string Role)
        {
            using (var db = new CfDb())
            {
                if (db.Users.Count(a => a.Username == Username || a.Email == Email) == 0
                    && !(String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(Realname) 
                    || String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(SchoolCode) || Grade == null)
                    && (Role == "Student" || Role == "Teacher"))
                {
                    try
                    {
                        var School = db.Schools.FirstOrDefault(a => a.SignupCode == SchoolCode);
                        if (School == null)
                        {
                            return 2;
                        }
                        WebSecurity.CreateUserAndAccount(Username, Password, new { School_Id = School.Id, Challenge = Classfinder.Database.User.GetNewChallenge(), Grade = Grade, Realname = Realname, Email = Email, JoinDate = DateTime.Now });
                        Roles.AddUserToRole(Username, Role);
                        return 0;
                    }
                    catch
                    {
                        return 1;
                    }
                }
                else if (db.Users.Count(a => a.Email == Email) > 0)
                {
                    return 4;
                }
                else if (db.Users.Count(u => u.Username == Username) > 0)
                {
                    return 5;
                }
                else
                {
                    return 3;
                }
            }
        }

        public bool ChangePassword(string CurrPass, string NewPass, string Username, string Challenge)
        {
            using(var db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Challenge == Challenge);
                if(user != null)
                {
                    var didWork = WebSecurity.ChangePassword(Username, CurrPass, NewPass);
                    return didWork;
                }
            }
            return false;
        }

        public string UpdateInfo(string Usernm, string Realname, string Email, string Username, string Challenge)
        {
            using (var db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Challenge == Challenge);
                if (user != null)
                {
                    var toret = "";
                    if (Usernm != Username)
                    {
                        if (!String.IsNullOrWhiteSpace(Usernm) && db.Users.Where(u => u.Username == Usernm).Count() == 0)
                        {
                            user.Username = Usernm;
                            toret += "UNM ";
                        }
                        else
                        {
                            toret += "USR ";
                        }
                    }
                    if (Realname != user.Realname)
                    {
                        if (!String.IsNullOrWhiteSpace(Realname))
                        {
                            user.Realname = Realname;
                        }
                        else
                        {
                            toret += "RNM ";
                        }
                    }
                    if (Email != user.Email)
                    {
                        if (!String.IsNullOrWhiteSpace(Email))
                        {
                            user.Email = Email;
                        }
                        else
                        {
                            toret += "EML";
                        }
                    }
                    db.SaveChanges();
                    return toret.Trim();
                }
            }
            return "BAAAAD";
        }

        public int ResetPass(string Email)
        {
            using (var db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == Email);
                if (user != null)
                {
                    //Thanks! http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
                    var Settings = Config.GetValues(new string[] { "SMTP Server", "SMTP Port", "SMTP User", "SMTP Pass" });
                    var mail = new MailMessage();
                    var SmtpServer = new SmtpClient(Settings["SMTP Server"]);

                    mail.From = new MailAddress("resetpass@classfinder.me", "Classfinder");
                    mail.To.Add(new MailAddress(Email, user.Realname));
                    mail.Subject = "Reset Your Password";
                    mail.Body = "Please visit http://classfinder.me/Account/ResetPass/" + WebSecurity.GeneratePasswordResetToken(user.Username) + " to reset your Classfinder password.";

                    SmtpServer.Port = Int32.Parse(Settings["SMTP Port"]);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Settings["SMTP User"], Settings["SMTP Pass"]);

                    SmtpServer.Send(mail);
                    return 0;
                }
            }
            return 1;
        }

        public bool ResetPassword(string Code, string Pass)
        {
            return WebSecurity.ResetPassword(Code, Pass);
        }
    }
}