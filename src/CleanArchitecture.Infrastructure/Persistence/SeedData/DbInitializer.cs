using System.Diagnostics;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence.SeedData;

public interface IDbInitializer
{
    Task SeedDataAsync();
}

public class DbInitializer : IDbInitializer
{
    private readonly ILogger<DbInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public DbInitializer(ILogger<DbInitializer> logger, ApplicationDbContext context, UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting Identity seed process...");
            await SeedRolesAsync();
            await SeedUsersAsync();
            _logger.LogInformation("✅ Identity seed completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while seeding identity data.");
        }
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            new Role
            {
                Name = Role.Admin,
                Description = "Administrator role with full permissions",
            },
            new Role
            {
                Name = Role.User,
                Description = "Standard user role with limited permissions",
            }
        };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Role {RoleName} created successfully", role.Name);
                }
                else
                {
                    _logger.LogError("Failed to create role {RoleName}: {Errors}",
                        role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        // Seed Admin User
        var adminEmail = "admin@clean-architecture-system.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true
            };

            var password = "Admin@123"; 
            var result = await _userManager.CreateAsync(adminUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, Role.Admin);
                _logger.LogInformation("Admin user created successfully");
            }
            else
            {
                _logger.LogError("Failed to create admin user: {Errors}", 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}