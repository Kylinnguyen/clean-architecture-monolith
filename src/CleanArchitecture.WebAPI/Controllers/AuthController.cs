using CleanArchitecture.WebAPI.Common;
using CleanArchitecture.Application.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.UseCases.Users.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;

namespace CleanArchitecture.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ApiResult> Register([FromBody] RegisterRequestDto request)
    {
        await _authService.RegisterAsync(request);
        return ApiResult.Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<ApiResult<UserResponseDto>> Login([FromBody] LoginRequestDto request,
        CancellationToken cancellationToke = default)
    {
        var result = await _authService.LoginAsync(request, cancellationToke);
        return ApiResult<UserResponseDto>.Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ApiResult> Logout()
    {
        var result = await _authService.LogoutAsync();
        if (!result.Succeeded)
        {
            return ApiResult.Fail("Logout failed", null, 400);
        }

        return ApiResult.Ok("Logged out successfully");
    }
}