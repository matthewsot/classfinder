using System;
using Microsoft.AspNet.Identity.Entity;
using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;

namespace Classfinder.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class UserAccount : User
    {

    }

    public class CfDb : IdentitySqlContext<UserAccount>
    {
        private static bool _created = false;
        
        public CfDb(IServiceProvider serviceProvider, IOptionsAccessor<DbContextOptions> optionsAccessor)
            : base(serviceProvider, optionsAccessor.Options.BuildConfiguration())
        {
            // Create the database and schema if it doesn't exist
            // This is a temporary workaround to create database until Entity Framework database migrations 
            // are supported in ASP.NET vNext
            if (!_created)
            {
                Database.EnsureCreated();
                _created = true;
            }
        }
    }
}