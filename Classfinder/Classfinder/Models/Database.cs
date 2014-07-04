using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Classfinder.Models
{
    public enum SignUpLevel
    {
        Unregistered = -1,
        Registered = 0,
        FirstSemesterScheduleEntered = 1,
        SecondSemesterScheduleEntered = 2,
    }

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

        public int GradYear { get; set; }
        public int SignUpLevel { get; set; }

        public virtual ICollection<Class> FirstSemester { get; set; }
        public virtual ICollection<Class> SecondSemester { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Class
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Period { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<UserAccount> StudentsInClassFirstSemester { get; set; }
        public virtual ICollection<UserAccount> StudentsInClassSecondSemester { get; set; }
    }

    public class CfDb : IdentityDbContext<UserAccount>
    {
        public CfDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().HasMany(user => user.FirstSemester).WithMany(@class => @class.StudentsInClassFirstSemester);
            modelBuilder.Entity<UserAccount>().HasMany(user => user.SecondSemester).WithMany(@class => @class.StudentsInClassSecondSemester);
            base.OnModelCreating(modelBuilder);
        }

        public static CfDb Create()
        {
            return new CfDb();
        }
    }
}