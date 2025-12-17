using CleanArchitecture.Domain.AppConfigurations.Token;
using CleanArchitecture.Infrastructure.Extensions;
using CleanArchitecture.Application.Interfaces.Services.Identity;
using CleanArchitecture.Application.Interfaces.Services.Token;
using CleanArchitecture.Infrastructure.Services.Identity;
using CleanArchitecture.Infrastructure.Services.Token;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.WebAPI.Middlewares;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Infrastructure.Persistence.SeedData;
using CleanArchitecture.WebAPI.ExceptionHandlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddMiniProfiler(
            ops => ops.RouteBasePath = "/profiler") // route in browser will be: /profiler/results
        .AddEntityFramework();
    
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "CleanArchitecture API",
            Version = "v1",
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter the token here (Example: Bearer token)"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

// Bind JwtSetting from appsettings.json
builder.Services.Configure<JwtSetting>(
    builder.Configuration.GetSection(nameof(JwtSetting)));

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsName", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Allow send cookie in request
    });
});

// Register services that are required by other services
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// Add db context 
builder.Services.AddAppDbContext(builder.Configuration);
// Add Identity
builder.Services.AddIdentityConfiguration();
// Register repository
builder.Services.AddRepositoriesConfiguration();

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Register Application service
builder.Services.AddApplication();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

builder.Services.AddControllers();

// Register ProblemDetails for exception handling
builder.Services.AddProblemDetails();

// Register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Apply migrations and seed data (Development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // Seed data 
        var dbInitializer = services.GetRequiredService<IDbInitializer>();
        await dbInitializer.SeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database");
    }
}

app.UseExceptionHandler();

// Middleware pipeline
app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("CorsName");
}


app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture API v1");
        x.DisplayRequestDuration();
        x.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    });
    app.UseMiniProfiler();
}

// Map endpoints
app.MapControllers();

app.Run();