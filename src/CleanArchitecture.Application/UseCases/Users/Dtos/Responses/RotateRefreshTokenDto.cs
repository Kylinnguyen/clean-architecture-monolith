namespace CleanArchitecture.Application.UseCases.Users.Dtos.Responses;

public class RotateRefreshTokenDto
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}