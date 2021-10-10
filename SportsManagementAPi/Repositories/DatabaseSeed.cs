using System.Collections.Generic;
using System.Linq;
using SportsManagementAPi.Domain.Models;
using SportsManagementAPi.Domain.Security;

namespace SportsManagementAPi.Repositories
{
    /// <summary>
    /// EF Core already supports database seeding throught overriding "OnModelCreating", but I decided to create a separate seed class to avoid 
    /// injecting IPasswordHasher into AppDbContext.
    /// To understand how to use database seeding into DbContext classes, check this link: https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
    /// </summary>
    public class DatabaseSeed
    {
        //public static void Seed(AppDbContext context, IPasswordHasher passwordHasher)
        //{
        //    context.Database.EnsureCreated();
            
        //    if (context.Roles.Count() == 0)
        //    {

        //        var roles = new List<Role>
        //        {
        //        new Role { Name = ApplicationRole.Common.ToString() },
        //        new Role { Name = ApplicationRole.Administrator.ToString() }
        //        };

        //        context.Roles.AddRange(roles);
        //        context.SaveChanges();
        //    }

        //    if (context.Users.Count() == 0)
        //    {
        //        var users = new List<Manager>
        //        {
        //            new Manager { Email = "admin@admin.com", Password = passwordHasher.HashPassword("12345678") },
        //            new Manager { Email = "common@common.com", Password = passwordHasher.HashPassword("12345678") },
        //        };

        //        users[0].UserRoles.Add(new UserRole
        //        {
        //            RoleId = context.Roles.SingleOrDefault(r => r.Name == ApplicationRole.Administrator.ToString()).Id
        //        });

        //        users[1].UserRoles.Add(new UserRole
        //        {
        //            RoleId = context.Roles.SingleOrDefault(r => r.Name == ApplicationRole.Common.ToString()).Id
        //        });

        //        context.Users.AddRange(users);
        //        context.SaveChanges();
        //    }
        //}
    }
}