using System.Security.Claims;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces.Services.Token;
using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;
using CleanArchitecture.Domain.AppConfigurations.Token;
using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identities;
using CleanArchitecture.Domain.Repositories.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Services.Token;

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenHelper _tokenHelper;
    private readonly JwtSetting _jwtSetting;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(UserManager<User> userManager, ITokenHelper tokenHelper, IOptions<JwtSetting> jwtSetting,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _tokenHelper = tokenHelper;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSetting = jwtSetting.Value;
    }

    public async Task<string> CreateAccessTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var roles = await _userManager.GetRolesAsync(user);
        // Generate tokens
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            // new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user?.Email),
            new(JwtRegisteredClaimNames.UniqueName, user?.UserName),
            new(JwtRegisteredClaimNames.Birthdate, user?.DateOfBirth.ToString() ?? string.Empty)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var accessToken = _tokenHelper.GenerateAccessToken(claims);
        return accessToken;
    }

    public async Task<string> CreateRefreshTokenAsync(User user, 
        CancellationToken cancellationToken = default)
    {
        var utcNow = DateTimeOffset.UtcNow;
        var refreshTokenTtl = _jwtSetting.ExpireTimeRefreshTokenInDays ?? 7; // time to live of refresh token 
        var token = _tokenHelper.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken()
        {
            UserId = user.Id,
            TokenHash = _tokenHelper.HashToken(token),
            CreatedTime = utcNow,
            ExpiredTime = utcNow.AddDays(refreshTokenTtl),
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken, true, cancellationToken);
        return token;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string rToken, CancellationToken cancellationToken = default)
    {
        var hashToken = _tokenHelper.HashToken(rToken);
        var refreshTokenQueryable = await _refreshTokenRepository.GetQueryableAsync();
        var refreshToken = await refreshTokenQueryable
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hashToken, cancellationToken: cancellationToken);
        
        if (refreshToken == null) throw new SecurityTokenException("Invalid refresh token");
        
        MarkRefreshTokenRevoked(refreshToken);
        await _refreshTokenRepository.UpdateAsync(refreshToken, true, cancellationToken);
        return true;
    }

    public async Task<bool> RevokeAllRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var refreshTokenQuery = await _refreshTokenRepository.GetQueryableAsync();
        var refreshTokens = await refreshTokenQuery
            .Where(x => x.UserId == userId 
                && x.RevokedTime == null)
            .ToListAsync(cancellationToken: cancellationToken);
        
        refreshTokens.ForEach(MarkRefreshTokenRevoked);
        await _refreshTokenRepository.UpdateRangeAsync(refreshTokens, true, cancellationToken);
        return true;
    }

    public async Task<RotateRefreshTokenDto> RotateRefreshToken(string accessToken, string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var userClaimPrincipals = _tokenHelper.GetPrincipleFromExpiredToken(accessToken);
        if (userClaimPrincipals == null)
        {
            throw new BadRequestException("Invalid token");
        }
        
        var userId = userClaimPrincipals.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new SecurityTokenException("Invalid access token");
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException(nameof(User), userId);
        
        var refreshTokenQuery = await _refreshTokenRepository.GetQueryableAsync();
        var hashRequestToken = _tokenHelper.HashToken(refreshToken);
        var refreshTokenEntity = await  refreshTokenQuery
            .FirstOrDefaultAsync(x => 
                x.UserId ==  user.Id &&
                x.TokenHash == hashRequestToken, cancellationToken);
        if (refreshTokenEntity == null)
            throw new SecurityTokenException("Refresh token not found");

        // Check token reuse attack
        if (refreshTokenEntity.IsRevoked)
        {
            await RevokeAllRefreshTokensAsync(user.Id, cancellationToken);
            throw new SecurityTokenException("Refresh token reuse detected");
        }

        // Check Refresh token expired → login again
        if (refreshTokenEntity.IsExpired)
            throw new UnauthorizedAccessException("Refresh token expired");
        
        MarkRefreshTokenRevoked(refreshTokenEntity);

        var newRefreshToken = await CreateRefreshTokenAsync(user, cancellationToken);
        var newAccessToken = await CreateAccessTokenAsync(user, cancellationToken);

        return new RotateRefreshTokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };
    }

    public async Task<User> GetUserByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var hashToken = _tokenHelper.HashToken(token);
        var refreshTokenQueryable = await _refreshTokenRepository.GetQueryableAsync();
        var refreshToken = await refreshTokenQueryable
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hashToken, cancellationToken: cancellationToken);

        return (refreshToken == null ? throw new SecurityTokenException("Invalid token") : refreshToken?.User)!;
    }

    private async Task RemoveOldRefreshTokens(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var refreshTokenQuery = await _refreshTokenRepository.GetQueryableAsync();
        var refreshTokens = await refreshTokenQuery
            .Where(x => x.UserId == userId)
            .Where(x => (x.IsExpired && x.ExpiredTime < DateTimeOffset.UtcNow)
                        || (x.IsRevoked && x.RevokedTime != null))
            .ToListAsync(cancellationToken: cancellationToken);

        await _refreshTokenRepository.RemoveRangeAsync(refreshTokens, true, cancellationToken);
    }
    
    private void MarkRefreshTokenRevoked(RefreshToken token)
    {
        token.RevokedTime = DateTimeOffset.UtcNow;
    }
}