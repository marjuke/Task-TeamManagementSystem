using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Persistance
{
    public class DbInitializer
    {
        public static async Task SeedData(AppDBContext context, UserManager<User> userManager)
        {
            // Create roles if they don't exist
            var roleManager = context.Roles;
            
            if (!roleManager.Any(r => r.Name == "Admin"))
            {
                context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }
            if (!roleManager.Any(r => r.Name == "Manager"))
            {
                context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "Manager", NormalizedName = "MANAGER" });
            }
            if (!roleManager.Any(r => r.Name == "Employee"))
            {
                context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" });
            }

            
            await context.SaveChangesAsync();

            if (!userManager.Users.Any())
            {
                var users = new List<(User user, string role)>
                {
                    (new User { DisplayName = "Admin", UserName = "Admin@demo.com", Email = "admin@demo.com" }, "Admin"),
                    (new User { DisplayName = "Manager", UserName = "Manager@demo.com", Email = "manager@demo.com" }, "Manager"),
                    (new User { DisplayName = "Employee", UserName = "Employee@demo.com", Email = "employee@demo.com" }, "Employee")
                };

                foreach (var (user, role) in users)
                {
                    var result = await userManager.CreateAsync(user, user.DisplayName + "123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            //data for statuses 
            if (!context.Statuses.Any())
            {
                var statuses = new List<Status>
                {
                    new Status { Name = "Todo" },
                    new Status { Name = "In Progress" },
                    new Status { Name = "Done" }
                };
                context.Statuses.AddRange(statuses);
            }


            await context.SaveChangesAsync();
        }
    }
}
