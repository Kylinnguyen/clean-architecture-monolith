using CleanArchitecture.Application.UseCases.Users.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Interfaces.Services.Identity;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto request, 
        CancellationToken cancellationToke = default);
    Task<UserResponseDto> LoginAsync(LoginRequestDto request, 
        CancellationToken cancellationToke = default);

    Task<IdentityResult> LogoutAsync();
    // Task ChangePasswordAsync(string email, string currentPassword, string newPassword);
    // Task ResetPasswordAsync(string email, string token, string newPassword);
    // Task ConfirmEmailAsync(string email, string token);
    // Task ForgotPasswordAsync(string email);
}