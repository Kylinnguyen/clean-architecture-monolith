using System.Security.Claims;
using CleanArchitecture.Application.Interfaces.Services.Token;
using CleanArchitecture.Domain.AppConfigurations.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Token;

public class TokenService : ITokenService
{
    private readonly JwtSetting _jwtSetting;

    public TokenService(IOptions<JwtSetting> jwtSetting)
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
            expires: utcNow.AddDays(Convert.ToDouble(_jwtSetting.ExpireTimeRefreshTokenInDays))
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

    public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
    {
        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret)),
            ValidateLifetime = false, // don't care about expiration time of token
        };

        var tokenHandler = new JwtSecurityTokenHandler(); // built-in .NET class to parse and validate JWTs
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out var securityToken);

        // Casts SecurityToken to JwtSecurityToken
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}