using Classfinder.Database;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using WebMatrix.WebData;

namespace Classfinder.Hubs.Admin
{
    public class SiteAdmin : Hub
    {
        public bool NewSchool(string SchoolName, string Code, string Locks, int Offset, string Username, string Challenge)
        {
            using (CfDb db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(a => a.Username == Username && a.Challenge == Challenge);
                if (user != null && Roles.IsUserInRole(Username, "SiteAdmin"))
                {
                    School newSchool = new School()
                    {
                        Name = SchoolName,
                        SignupCode = Code,
                        PeriodOffset = Offset
                    };
                    db.Schools.Add(newSchool);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool CreateUser(string Username, string Pass, string Realnm, string Email, string Permissions, string SchoolCode, int Grade, string MyUsername, string Challenge)
        {
            using (CfDb db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(a => a.Username == MyUsername && a.Challenge == Challenge);
                if (user != null && Roles.IsUserInRole(MyUsername, "SiteAdmin"))
                {
                    if (String.IsNullOrWhiteSpace(Permissions))
                    {
                        Permissions = "Student";
                    }
                    //copy/paste from account hub
                    //TODO: write generic add account function
                    if (db.Users.Count(a => a.Username == Username) == 0
                    && !(String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Pass) || String.IsNullOrWhiteSpace(Realnm)
                    || String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(SchoolCode) || Grade == null))
                    {
                        try
                        {
                            var School = db.Schools.FirstOrDefault(a => a.SignupCode == SchoolCode);
                            if (School == null)
                            {
                                return false;
                            }
                            WebSecurity.CreateUserAndAccount(Username, Pass, new { School_Id = School.Id, Challenge = Classfinder.Database.User.GetNewChallenge(), Grade = Grade, Realname = Realnm, Email = Email, JoinDate = DateTime.Now });
                            var SplitPerms = Permissions.Split(',');
                            for (var i = 0; i < SplitPerms.Length; i++)
                            {
                                SplitPerms[i] = SplitPerms[i].Trim();
                                if (Roles.RoleExists(SplitPerms[i]))
                                {
                                    Roles.AddUserToRole(Username, SplitPerms[i]);
                                }
                            }
                            return true;
                        }
                        catch { }
                    }
                }
                return false;
            }
        }
        public string GetPerms(string Usernm, string Username, string Challenge)
        {
            using (CfDb db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Challenge == Challenge);
                if (user != null && Roles.IsUserInRole(Username, "SiteAdmin"))
                {
                    var userToPerm = db.Users.FirstOrDefault(u => u.Username == Usernm);
                    if (userToPerm != null)
                    {
                        return string.Join(", ", Roles.GetRolesForUser(Usernm));
                    }
                }
            }
            return "Error";
        }
        public bool SetPerms(string Usernm, string Perms, string Username, string Challenge)
        {
            using (CfDb db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username && u.Challenge == Challenge);
                if (user != null && Roles.IsUserInRole(Username, "SiteAdmin"))
                {
                    var userToPerm = db.Users.FirstOrDefault(u => u.Username == Usernm);
                    if (userToPerm != null)
                    {
                        var SplitPerms = Perms.Split(',');
                        for (var i = 0; i < SplitPerms.Length; i++)
                        {
                            SplitPerms[i] = SplitPerms[i].Trim();
                            if (Roles.RoleExists(SplitPerms[i]))
                            {
                                Roles.AddUserToRole(Usernm, SplitPerms[i]);
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        public List<Class> CombineClasses(string Username, string Challenge)
        {
            using (CfDb db = new CfDb())
            {
                var x = db.Classes.Where(c => c.Teacher.Contains(", ")).ToList();
                var SimilarClasses = db.Classes.Where(first => db.Classes.Count(second =>
                    first.Id != second.Id &&
                    first.Period == second.Period &&
                    first.Name == second.Name &&
                    first.Teacher == second.Teacher &&
                    first.ClassSchool == second.ClassSchool) > 0).ToList<Class>();
                var DoneSoFar = new List<Class>();
                foreach (var ThisClass in SimilarClasses)
                {
                    try
                    {
                        if (DoneSoFar.Count(a => a.Period == ThisClass.Period && a.Teacher == ThisClass.Teacher && a.ClassSchool.Id == ThisClass.ClassSchool.Id) > 0)
                        {
                            continue;
                        }
                        else
                        {
                            DoneSoFar.Add(ThisClass);
                            var SecondClass = SimilarClasses.FirstOrDefault(first =>
                                first.Id != ThisClass.Id &&
                                first.Period == ThisClass.Period &&
                                first.Name == ThisClass.Name &&
                                first.Teacher == ThisClass.Teacher &&
                                first.ClassSchool.Id == ThisClass.ClassSchool.Id);
                            if (SecondClass != null)
                            {
                                foreach (User user in ThisClass.FirstSemStudents)
                                {
                                    SecondClass.FirstSemStudents.Add(user);
                                }
                                foreach (User user in ThisClass.SecondSemStudents)
                                {
                                    SecondClass.SecondSemStudents.Add(user);
                                }
                            }
                            db.Classes.Remove(ThisClass);
                        }
                    }
                    catch {
                        continue;
                    }
                }
                db.SaveChanges();
                return SimilarClasses;
            }
        }
    }
}