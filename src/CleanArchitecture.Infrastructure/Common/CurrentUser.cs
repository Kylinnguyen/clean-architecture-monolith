using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.Common;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => User?.FindFirstValue(ClaimTypes.Name);
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);
    public string? FirstName => User?.FindFirstValue(ClaimTypes.GivenName)
                                ?? User?.FindFirstValue("given_name");

    public string? LastName => User?.FindFirstValue(ClaimTypes.Surname)
                               ?? User?.FindFirstValue("family_name");

    public string? PhoneNumber => User?.FindFirstValue(ClaimTypes.MobilePhone)
                                  ?? User?.FindFirstValue("phone_number");
    public IEnumerable<string> Roles  
        => User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value) ?? [];
    public bool HasRole(string role) 
        => Roles.Any(x => x == role);

    public IEnumerable<Claim> Claims
        => User?.Claims ?? [];

    public bool HasClaim(string type, string? value = null)
    {
        if (value == null) return User?.HasClaim(x => x.Type == type) ?? false;
        return User?.HasClaim(type, value) ?? false;
    }
}