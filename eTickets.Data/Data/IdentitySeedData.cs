using eTickets.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eTickets.Data
{
    public static class IdentitySeedData
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { "Administrator", "Customer" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true, FullName = "Administrator" };
                var createResult = await userManager.CreateAsync(admin, password);
                if (!createResult.Succeeded) return;
            }

            if (!await userManager.IsInRoleAsync(admin, "Administrator"))
                await userManager.AddToRoleAsync(admin, "Administrator");
        }
    }
}
