using System.Security.Claims;
using CleanArchitecture.Application.Interfaces.Services.Token;
using CleanArchitecture.Domain.AppConfigurations.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Token;

public class TokenHelper : ITokenHelper
{
    private readonly JwtSetting _jwtSetting;

    public TokenHelper(IOptions<JwtSetting> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var utcNow = DateTime.UtcNow;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: utcNow.AddMinutes(Convert.ToDouble(_jwtSetting.ExpireTimeAccessTokenInMinutes))
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        var hashBytes = sha256.ComputeHash(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public ClaimsPrincipal? GetPrincipleFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler(); // built-in .NET class to parse and validate JWTs
        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret)),
            ValidateLifetime = false, // don't care about expiration time of token
            ValidIssuer = _jwtSetting?.Issuer,
            ValidAudience = _jwtSetting?.Audience,
            ClockSkew = TimeSpan.Zero,
        };

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out var securityToken);

        // Casts SecurityToken to JwtSecurityToken
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
            // throw new SecurityTokenException("Invalid access token");
        }

        return principal;
    }
}