using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "John";
        private const string adminPassword = "Secret123$";

        private const string managerUser = "Paul";
        private const string managerPassword = "Secret456$";

        private const string adminRoleName = "Admin";
        private const string managerRoleName = "Manager";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            // Create the Role
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            // Admin Role
            IdentityRole adminRole = await roleManager.FindByNameAsync(adminRoleName);

            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                await roleManager.CreateAsync(adminRole);
            }


            UserManager<IdentityUser> userManager =
                app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();

            // create the admin user
            IdentityUser user = await userManager.FindByIdAsync(adminUser);

            if (user == null)
            {
                user=new IdentityUser(adminUser);
                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(user, adminRoleName)))
                {
                    await userManager.AddToRoleAsync(user, adminRoleName);
                }
            }

            // Manager Role
            IdentityRole managerRole = await roleManager.FindByNameAsync(managerRoleName);

            if (managerRole == null)
            {
                managerRole = new IdentityRole(managerRoleName);
                await roleManager.CreateAsync(managerRole);
            }

            // create the manager user
            IdentityUser managerUserIdentity = await userManager.FindByIdAsync(managerUser);

            if (managerUserIdentity == null)
            {
                managerUserIdentity = new IdentityUser(managerUser);
                await userManager.CreateAsync(managerUserIdentity, managerPassword);
                await userManager.AddToRoleAsync(managerUserIdentity, managerRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(managerUserIdentity, managerRoleName)))
                {
                    await userManager.AddToRoleAsync(managerUserIdentity, managerRoleName);
                }
            }
        }

    }
}
