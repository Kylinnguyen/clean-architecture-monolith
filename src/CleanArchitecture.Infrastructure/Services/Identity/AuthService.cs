using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces.Services.Identity;
using CleanArchitecture.Application.Interfaces.Services.Token;
using CleanArchitecture.Application.UseCases.Users.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToke = default)
    {
        // Check if email exists
        if (await _userManager.FindByEmailAsync(request.Email) != null)
            throw new BadRequestException("Email is already taken");

        // Check if username exists
        if (await _userManager.FindByNameAsync(request.UserName) != null)
            throw new BadRequestException("Username is already taken");
        
        var user = request.MapToEntity();
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new BusinessException(result.Errors.First().Description, details: errors );
        }
        await _userManager.AddToRoleAsync(user, Role.User);
        
        return true;
    }

    public async Task<UserResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("Wrong email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Wrong email or password");
        }

        var accessToken = await _tokenService.CreateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, cancellationToken);

        var userResponseDto = new UserResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            // Roles = roles.ToList(),
            User = new UserInfoDto
            {
                Id = user.Id,
                Email = user?.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }
        };

        return userResponseDto;
    }

    public async Task<IdentityResult> LogoutAsync(string refreshToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(refreshToken);
        await _signInManager.SignOutAsync();
        return IdentityResult.Success;
    }
}