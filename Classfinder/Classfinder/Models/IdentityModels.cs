using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Classfinder.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class UserAccount : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserAccount> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public int Grade { get; set; }
        public virtual School School { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public Schedule GetFirstSemesterSchedule()
        {
            return Schedules.FirstOrDefault(sched => sched.Term == 1);
        }

        public Schedule GetSecondSemesterSchedule()
        {
            return Schedules.FirstOrDefault(sched => sched.Term == 2);
        }
    }

    public class Schedule
    {
        public int Id { get; set; }
        public int Term { get; set; }
        public virtual UserAccount User { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }

    public enum SchoolType
    {
        HighSchool,
        MiddleSchool,
        ElementarySchool
    }

    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinGrade { get; set; }
        public int MaxGrade { get; set; }
        public int Terms { get; set; }
        public virtual ICollection<UserAccount> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }

        public School(string Name = "", SchoolType schoolType = SchoolType.HighSchool)
        {
            this.Name = Name;
            Terms = 2;
            switch(schoolType)
            {
                case SchoolType.HighSchool:
                    MinGrade = 9;
                    MaxGrade = 12;
                    break;
                case SchoolType.MiddleSchool:
                    MinGrade = 6;
                    MaxGrade = 8;
                    break;
                case SchoolType.ElementarySchool:
                    Terms = 1;
                    MinGrade = 0;
                    MaxGrade = 5;
                    break;
            }

        }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual School School { get; set; }
    }

    public class Class
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Period { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Schedule> SchedulesWithClass { get; set; }
    }

    public class CfDb : IdentityDbContext<UserAccount>
    {
        public CfDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().HasMany(user => user.Schedules).WithRequired(schedule => schedule.User);

            modelBuilder.Entity<Schedule>().HasMany(schedule => schedule.Classes).WithMany(@class => @class.SchedulesWithClass);

            modelBuilder.Entity<School>().HasMany(school => school.Students).WithRequired(student => student.School);
            modelBuilder.Entity<School>().HasMany(school => school.Teachers).WithRequired(teacher => teacher.School);
            base.OnModelCreating(modelBuilder);
        }

        public static CfDb Create()
        {
            return new CfDb();
        }
    }
}