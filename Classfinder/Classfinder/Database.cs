using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Classfinder.Database
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Realname { get; set; }
        [Required]
        public DateTime JoinDate { get; set; }
        public virtual ICollection<Class> FirstSem { get; set; }
        public virtual ICollection<Class> SecondSem { get; set; }
        public virtual School School { get; set; }
        public int Grade { get; set; }
        [Required]
        public string Challenge { get; set; }
        public static User GetFromUsername(string username)
        {
            using (var db = new CfDb())
            {
                var toRet = db.Users.FirstOrDefault(u => u.Username == username);
                return toRet;
            }
        }
        public static string GetNewChallenge()
        {
            var path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            var pathTwo = Path.GetRandomFileName();
            pathTwo = pathTwo.Replace(".", ""); // Remove period.
            return path + pathTwo;
        }
    }
    public class Class
    {
        public int Id { get; set; }
        public int Period { get; set; }
        public string Name { get; set; }
        public virtual School ClassSchool { get; set; }
        public virtual ICollection<User> FirstSemStudents { get; set; }
        public virtual ICollection<User> SecondSemStudents { get; set; }
        public string Teacher { get; set; }
    }
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SignupCode { get; set; }
        /*
         * Lock codes, seperated by commas
         * NEWCLASS - locks ability to create a new class
         * CLASSMAN - locks ability to manage classes at all
         */
        public int PeriodOffset { get; set; }
    }
    public class Setting
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class CfDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Setting> Config { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Class> Classes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(e => e.FirstSem).WithMany(c => c.FirstSemStudents);
            modelBuilder.Entity<User>().HasMany(e => e.SecondSem).WithMany(c => c.SecondSemStudents);
            base.OnModelCreating(modelBuilder);
        }
    }
}