using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.Areas.Identity;

namespace StudentEmploymentPortal.DataSeeding
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

                await SeedRolesAsync(roleManager);
                await SeedApproverAsync(userManager);
                await SeedAdminAsync(userManager);

                // Perform other data seeding if needed
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(SD.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Admin));
            }

            if (!await roleManager.RoleExistsAsync(SD.Approver))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Approver));
            }

            // Add other roles if needed
        }

        private static async Task SeedApproverAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "approver0@gmail.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "approver",
                    Surname = "approver",
                    Telephone = "0116789876",
                    PhoneNumber = "0116789876",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Approver01#");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Approver);
                }
            }
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admin0@gmail.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "admin",
                    Surname = "admin",
                    Telephone = "0116789876",
                    PhoneNumber = "0116789876",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin01#");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Admin);
                }
            }
        }
    }
}
