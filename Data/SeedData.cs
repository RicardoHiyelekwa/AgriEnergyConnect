using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeedDataAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Create roles
            var roles = new[] { "Farmer", "Employee" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }

            // Create default employee
            var employeeEmail = "employee@agri.com";
            var employee = await userManager.FindByEmailAsync(employeeEmail);
            if (employee == null)
            {
                employee = new ApplicationUser { UserName = employeeEmail, Email = employeeEmail, EmailConfirmed = true };
                await userManager.CreateAsync(employee, "Employee@1234");
                await userManager.AddToRoleAsync(employee, "Employee");
            }

            // Create default farmer user
            var farmerEmail = "farmer@agri.com";
            var farmerUser = await userManager.FindByEmailAsync(farmerEmail);
            if (farmerUser == null)
            {
                farmerUser = new ApplicationUser { UserName = farmerEmail, Email = farmerEmail, EmailConfirmed = true };
                await userManager.CreateAsync(farmerUser, "Farmer@1234");
                await userManager.AddToRoleAsync(farmerUser, "Farmer");
            }

            // Ensure farmer profile exists and sample products
            if (!context.Farmers.Any(f => f.Email == farmerEmail))
            {
                var farmer = new Farmer
                {
                    Name = "Sample Farmer",
                    Email = farmerEmail,
                    IdentityUserId = farmerUser.Id
                };
                context.Farmers.Add(farmer);
                await context.SaveChangesAsync();

                context.Products.AddRange(
                    new Product { Name = "Tomatoes", Category = "Vegetable", ProductionDate = DateTime.UtcNow.AddDays(-10), FarmerId = farmer.Id },
                    new Product { Name = "Corn", Category = "Grain", ProductionDate = DateTime.UtcNow.AddDays(-30), FarmerId = farmer.Id },
                    new Product { Name = "Basil", Category = "Herb", ProductionDate = DateTime.UtcNow.AddDays(-5), FarmerId = farmer.Id }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
