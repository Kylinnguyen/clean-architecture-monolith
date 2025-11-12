using System.Security.Claims;

namespace CleanArchitecture.Application.Interfaces.Services.Token;

public interface ITokenService
{
    /// <summary>
    /// Generates a signed JWT access token containing the specified claims.
    /// Typically short-lived and used for authenticating API requests.
    /// </summary>
    /// <param name="claims">The collection of user claims to embed in the token.</param>
    /// <returns>A signed JWT access token as a string.</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Generates a secure refresh token.
    /// Typically long-lived and used to obtain new access tokens without re-authenticating.
    /// </summary>
    /// <returns>A refresh token as a string.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Extracts the <see cref="ClaimsPrincipal"/> (user identity and claims) 
    /// from an expired JWT token without validating its expiration.
    /// Useful for refresh token workflows where claims are needed even if the access token is expired.
    /// </summary>
    /// <param name="token">The expired JWT token.</param>
    /// <returns>
    /// The <see cref="ClaimsPrincipal"/> if the token is valid (ignoring expiration), 
    /// otherwise <c>null</c>.
    /// </returns>
    ClaimsPrincipal GetPrincipleFromExpiredToken(string token);
}