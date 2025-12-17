using System.Diagnostics;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        // Define all users to seed
        var seedUsers = new List<SeedUserDto>
        {
            new() {
                UserName = "admin",
                Email = "admin@ca.com",
                FirstName = "System",
                LastName = "Administrator",
                DateOfBirth = new DateTime(2000, 2, 20),
                Password = "Admin@123",
                Role = Role.Admin
            },
            new() {
                UserName = "user1",
                Email = "user1@ca.com",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1995, 5, 15),
                Password = "User@123",
                PhoneNumber = "0811234556",
                Role = Role.User
            },
            new() {
                UserName = "user2",
                Email = "user2@ca.com",
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1992, 8, 22),
                Password = "User@123",
                Role = Role.User
            },
            new(){
                UserName = "user3",
                Email = "user3@ca.com",
                FirstName = "Bob",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1998, 11, 10),
                Password = "User@123",
                PhoneNumber = "0939876543",
                Role = Role.User
            }
        };

        // Get all emails to check
        var emails = seedUsers.Select(u => u.Email).ToList();

        // Query all existing users by email list in a single database call
        var existingUsers = await _context.Users
            .Where(u => u.Email != null && emails.Contains(u.Email))
            .Select(u => u.Email!)
            .ToListAsync();

        var existingEmails = new HashSet<string>(existingUsers, StringComparer.OrdinalIgnoreCase);

        // Create users that don't exist
        foreach (var userData in seedUsers)
        {
            if (existingEmails.Contains(userData.Email))
            {
                continue;
            }

            var user = new User
            {
                UserName = userData.UserName,
                Email = userData.Email,
                EmailConfirmed = true,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                IsActive = true,
                DateOfBirth = userData.DateOfBirth,
                PhoneNumber = userData.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userData.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userData.Role);
                _logger.LogInformation("User {UserName} created successfully with role {Role}",
                    userData.UserName, userData.Role);
            }
            else
            {
                _logger.LogError("Failed to create user {UserName}: {Errors}",
                    userData.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}