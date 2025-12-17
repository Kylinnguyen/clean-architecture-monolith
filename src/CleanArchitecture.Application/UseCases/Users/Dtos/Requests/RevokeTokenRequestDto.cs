namespace CleanArchitecture.Application.UseCases.Users.Dtos.Requests;

public class RevokeTokenRequestDto
{
    public string? RefreshToken { get; set; }
}