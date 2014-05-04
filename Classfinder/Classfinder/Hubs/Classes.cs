using Classfinder.Database;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classfinder.Hubs
{
    public class Classes : Hub
    {
        private List<Class> ParseSched(string[] Sched, School School, CfDb db)
        {
            List<Class> Parsed = new List<Class>();
            foreach (var semClass in Sched)
            {
                if (semClass == null)
                {
                    continue;
                }
                var Split = semClass.Split(',');
                var Pd = int.Parse(Split[0]);
                if (Split.Length == 1)
                {
                    Class newClass = new Class()
                    {
                        Period = Pd,
                    };
                    Parsed.Add(newClass);
                    continue;
                }
                var Name = Split[1].Replace("COMMA", ",").Trim();
                var Teacher = Split[2].Replace("COMMA", ",").Trim();
                Class thisClass = db.Classes.FirstOrDefault(c => c.Period == Pd && c.Name == Name && c.Teacher == Teacher);
                if (thisClass != null)
                {
                    Parsed.Add(thisClass);
                }
                else
                {
                    Class newClass = new Class()
                    {
                        Teacher = Teacher,
                        Name = Name,
                        Period = Pd,
                        ClassSchool = School
                    };
                    db.Classes.Add(newClass);
                    Parsed.Add(newClass);
                }
            }
            db.SaveChanges();
            return Parsed;
        }
        public string UpdateClasses(string[] FirstSem, string[] SecondSem, string Username, string Challenge)
        {
            try
            {
                using (CfDb db = new CfDb())
                {
                    User user = db.Users.FirstOrDefault(u => u.Username == Username && u.Challenge == Challenge);
                    List<Class> FirstSemester = ParseSched(FirstSem, user.School, db); //apparently we don't need db to be a ref http://stackoverflow.com/questions/6001016/why-c-sharp-dont-let-to-pass-a-using-variable-to-a-function-as-ref-or-out
                    List<Class> SecondSemester = ParseSched(SecondSem, user.School, db);
                    if (user != null)
                    {
                        foreach (var FirstClass in FirstSemester)
                        {
                            if (!user.FirstSem.Contains(FirstClass)) //should probably use eager loading here, still need to profile it and see how EF handles this stuff
                            {
                                var prevClass = user.FirstSem.FirstOrDefault(c => c.Period == FirstClass.Period);
                                if (prevClass != null)
                                {
                                    user.FirstSem.Remove(prevClass);
                                    if (prevClass.FirstSemStudents.Count() < 2 && prevClass.SecondSemStudents.Count() < 2) //apparently we're still counted, even if we removed from firstsem
                                    {
                                        db.Classes.Remove(prevClass); //remove the class if there are no more students
                                    }
                                }
                                if (FirstClass.Name != null)
                                {
                                    user.FirstSem.Add(FirstClass);
                                }
                            }
                            //otherwise, we're good! leave that class alone, it's already set
                        }
                        foreach (var SecondClass in SecondSemester)
                        {
                            if (!user.SecondSem.Contains(SecondClass)) //should probably use eager loading here, still need to profile it and see how EF handles this stuff
                            {
                                var prevClass = user.SecondSem.FirstOrDefault(c => c.Period == SecondClass.Period);
                                if (prevClass != null)
                                {
                                    user.SecondSem.Remove(prevClass);
                                    if (prevClass.FirstSemStudents.Count() < 2 && prevClass.SecondSemStudents.Count() < 2)
                                    {
                                        db.Classes.Remove(prevClass); //remove the class if there are no more students
                                    }
                                }
                                if (SecondClass.Name != null)
                                {
                                    user.SecondSem.Add(SecondClass);
                                }
                            }
                            //otherwise, we're good! leave that class alone, it's already set
                        }
                        db.SaveChanges();
                    }
                }
                return "true";
            }
            catch (Exception e)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(FirstSem);
            }
        }
    }
}