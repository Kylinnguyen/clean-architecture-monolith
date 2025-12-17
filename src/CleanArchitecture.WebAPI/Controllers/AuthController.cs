using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.WebAPI.Common;
using CleanArchitecture.Application.Interfaces.Services.Identity;
using CleanArchitecture.Application.Interfaces.Services.Token;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.UseCases.Users.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    
    public AuthController(
        IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ApiResult> Register([FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await _authService.RegisterAsync(request, cancellationToken);
        return ApiResult.Ok("User registered successfully");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ApiResult<UserResponseDto>> Login([FromBody] LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
        return ApiResult<UserResponseDto>.Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ApiResult> Logout(CancellationToken cancellationToken = default)
    {
        var refreshToken = GetRefreshTokenFromCookie();
        var result = await _authService.LogoutAsync(refreshToken);
        if (!result.Succeeded)
        {
            return ApiResult.Fail("Logout failed", null, StatusCodes.Status400BadRequest);
        }

        return ApiResult.Ok("Logged out successfully");
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ApiResult<RotateRefreshTokenDto>> Refresh([FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = GetRefreshTokenFromCookie();
        var result = await _tokenService.RotateRefreshToken(request.AccessToken, 
            refreshToken, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
        return ApiResult<RotateRefreshTokenDto>.Ok(result);
    }

    [HttpPost("revoke-token")]
    public async Task<ApiResult> RevokeToken([FromBody] RevokeTokenRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = request.RefreshToken ?? GetRefreshTokenFromCookie();
        await _tokenService.RevokeRefreshTokenAsync(refreshToken, cancellationToken);
        return ApiResult.Ok();
    }
    
    [HttpPost("revoke-all-tokens")]
    public async Task<ApiResult> RevokeAllTokens([FromBody] RevokeAllTokensRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await _tokenService.RevokeAllRefreshTokensAsync(request.UserId, cancellationToken);
        return ApiResult.Ok();
    }

    #region Method helpers

    private void SetRefreshTokenCookie(string refreshToken)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private string GetRefreshTokenFromCookie()
    {
        return Request.Cookies["refreshToken"]
               ?? throw new NotFoundException(nameof(RefreshToken), "Refresh token is required");
    }

    #endregion
}