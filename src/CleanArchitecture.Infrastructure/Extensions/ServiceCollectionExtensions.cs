using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });
        
        
        
        // Register Database Seeder
        services.AddScoped<IDbInitializer, DbInitializer>();
    }

    public static void AddIdentityConfiguration(this IServiceCollection services)
    {
        // Use AddIdentity -> Full config of Identity: SignInManager, UserManager, RoleManager, IdentityRole,...
        services.AddIdentity<User, Role>(op =>
            {
                // password config 
                op.Password.RequireDigit = true;
                op.Password.RequireLowercase = true;
                op.Password.RequireUppercase = true;
                op.Password.RequireNonAlphanumeric = true;
                op.Password.RequiredLength = 8;

                // user config
                op.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Use AddIdentitySCore: lightweight Identity: UserManager. Not contains RoleManager, SigninManager, 
        
        // services.ConfigureApplicationCookie(options =>
        // {
        //     // Cookie settings
        //     options.Cookie.Name = "CLEAN_ARCHITECTURE_APP";
        //     options.Cookie.HttpOnly = true;
        //     options.Cookie.SameSite = SameSiteMode.None;
        //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //     options.ExpireTimeSpan = TimeSpan.FromDays(1);
        //
        //     options.LoginPath = "/api/auth/login";
        //     options.SlidingExpiration = true;
        //     options.Events = new CookieAuthenticationEvents
        //     {
        //         OnRedirectToLogin = redirectContext =>
        //         {
        //             redirectContext.HttpContext.Response.StatusCode = 401;
        //             return Task.CompletedTask;
        //         }
        //     };
        // });
    }
}