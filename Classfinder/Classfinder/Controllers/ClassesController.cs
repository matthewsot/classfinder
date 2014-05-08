using Classfinder.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebMatrix.WebData;

namespace Classfinder.Controllers
{
    [Authorize]
    public class ClassesController : ApiController
    {
        private CfDb db = new CfDb();

        private IEnumerable<Class> ParseSched(IEnumerable<string> sched, School school)
        {
            var parsed = new List<Class>();
            foreach (var semClass in sched)
            {
                if (semClass == null)
                {
                    continue;
                }
                var split = semClass.Split(',');
                var period = int.Parse(split[0]);
                if (split.Length == 1)
                {
                    var newClass = new Class
                    {
                        Period = period,
                    };
                    parsed.Add(newClass);
                    continue;
                }
                var name = split[1].Replace("COMMA", ",").Trim();
                var teacher = split[2].Replace("COMMA", ",").Trim();
                var thisClass = db.Classes.FirstOrDefault(c => c.Period == period && c.Name == name && c.Teacher == teacher);
                if (thisClass != null)
                {
                    parsed.Add(thisClass);
                }
                else
                {
                    var newClass = new Class
                    {
                        Teacher = teacher,
                        Name = name,
                        Period = period,
                        ClassSchool = school
                    };
                    db.Classes.Add(newClass);
                    parsed.Add(newClass);
                }
            }
            db.SaveChanges();
            return parsed;
        }

        [Route("API/Classes/Update")]
        public IHttpActionResult UpdateClasses(IEnumerable<string> firstSem, IEnumerable<string> secondSem)
        {
            try
            {
                var user = db.Users.Find(WebSecurity.CurrentUserId);
                if (user != null)
                {
                    var firstSemester = ParseSched(firstSem, user.School);
                    var secondSemester = ParseSched(secondSem, user.School);

                    foreach (var firstClass in firstSemester)
                    {
                        if (user.FirstSem.Contains(firstClass)) continue;

                        var prevClass = user.FirstSem.FirstOrDefault(c => c.Period == firstClass.Period);
                        if (prevClass != null)
                        {
                            user.FirstSem.Remove(prevClass);
                            if (prevClass.FirstSemStudents.Count() < 2 && prevClass.SecondSemStudents.Count() < 2) //apparently we're still counted, even if we removed from firstsem
                            {
                                db.Classes.Remove(prevClass); //remove the class if there are no more students
                            }
                        }
                        if (firstClass.Name != null)
                        {
                            user.FirstSem.Add(firstClass);
                        }
                        //otherwise, we're good! leave that class alone, it's already set
                    }

                    foreach (var secondClass in secondSemester)
                    {
                        if (user.SecondSem.Contains(secondClass)) continue;

                        var prevClass = user.SecondSem.FirstOrDefault(c => c.Period == secondClass.Period);
                        if (prevClass != null)
                        {
                            user.SecondSem.Remove(prevClass);
                            if (prevClass.FirstSemStudents.Count() < 2 && prevClass.SecondSemStudents.Count() < 2)
                            {
                                db.Classes.Remove(prevClass); //remove the class if there are no more students
                            }
                        }
                        if (secondClass.Name != null)
                        {
                            user.SecondSem.Add(secondClass);
                        }
                        //otherwise, we're good! leave that class alone, it's already set
                    }
                    db.SaveChanges();
                    return Ok("true");
                }
            }
            catch (Exception e)
            {
                //wtf
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(firstSem));
            }
            return Ok("false");
        }

        [Route("API/Peers")]
        public IHttpActionResult GetPeers()
        {
            var retting = new List<dynamic>();
            var user = db.Users.Find(WebSecurity.CurrentUserId);
            if (user == null)
            {
                return Ok("bad");
            }

            retting.Add(user.Realname); //i'm not even gonna...wtf...why ?
            if (user.FirstSem != null)
            {
                retting.Add(user.FirstSem.Select(a => new
                {
                    IsFirst = true,
                    a.Period,
                    a.Id,
                    a.Teacher,
                    a.Name,
                    Peers = a.FirstSemStudents.Select(u => new
                    {
                        u.Realname,
                        u.Username
                    }).ToArray()
                }).OrderBy(a => a.Period).ToArray());
            }

            if (user.SecondSem != null)
            {
                retting.Add(user.SecondSem.Select(a => new
                {
                    IsFirst = false,
                    a.Period,
                    a.Id,
                    a.Teacher,
                    a.Name,
                    Peers = a.SecondSemStudents.Select(u => new
                    {
                        u.Realname,
                        u.Username
                    }).ToArray()
                }).OrderBy(a => a.Period).ToArray());
            }
            var res = retting.ToArray();
            return Ok(res);
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
