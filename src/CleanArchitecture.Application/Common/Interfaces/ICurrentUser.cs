using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    string? PhoneNumber { get; }
    
    IEnumerable<string> Roles { get; }
    bool HasRole(string role);
    IEnumerable<Claim> Claims { get; }
    bool HasClaim(string type, string? value = null);
}