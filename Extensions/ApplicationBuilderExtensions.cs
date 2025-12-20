using FirstAidAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                // Tạo roles
                string[] roleNames = { "Admin", "User" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                        Console.WriteLine($"✅ Created role: {roleName}");
                    }
                }

                // Gán Admin role
                var adminEmail = "22110401@student.hcmute.edu.vn";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser != null)
                {
                    var currentRoles = await userManager.GetRolesAsync(adminUser);
                    if (!currentRoles.Contains("Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        Console.WriteLine($"✅ Assigned Admin role to {adminUser.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}