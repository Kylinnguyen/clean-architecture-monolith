using CleanArchitecture.Application.UseCases.Users.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Application.Interfaces.Services.Token;

public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(User user, CancellationToken cancellationToken = default);
    Task<string> CreateRefreshTokenAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeAllRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<RotateRefreshTokenDto> RotateRefreshToken(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
    Task<User> GetUserByTokenAsync(string token, CancellationToken cancellationToken = default);
}