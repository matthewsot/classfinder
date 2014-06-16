using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

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
    }

    public class School
    {
        public string Name { get; set; }
        public virtual ICollection<UserAccount> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }

    public class Teacher
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual School School { get; set; }
    }

    public class Class
    {
        public string Subject { get; set; }
        public int Period { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<UserAccount> Students { get; set; }
    }

    public class CfDb : IdentityDbContext<UserAccount>
    {
        public CfDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static CfDb Create()
        {
            return new CfDb();
        }
    }
}