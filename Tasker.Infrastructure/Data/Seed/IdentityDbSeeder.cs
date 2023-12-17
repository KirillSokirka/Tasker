using Microsoft.AspNetCore.Identity;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Infrastructure.Data.Seed;

public static class IdentityDbSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "SuperAdmin", "Admin", "User" };
        
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task SeedTestUserAsync(UserManager<ApplicationUser> userManager)
    {
        var userName = "testuser@example.com";
        var password = "Test@123";

        if (await userManager.FindByNameAsync(userName) == null)
        {
            var user = new ApplicationUser()
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                string[] roleNames = { "SuperAdmin", "Admin", "User" };

                foreach (var roleName in roleNames)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}