namespace CleanArchitecture.Domain.AppConfigurations.Token;

public class JwtSetting
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    // public int ExpirationTimeInMinutes { get; set; }
    public string? ExpireTimeAccessTokenInMinutes { get; set; }
    public string? ExpireTimeRefreshTokenInDays { get; set; }
}