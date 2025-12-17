namespace CleanArchitecture.Domain.AppConfigurations.Token;

public class JwtSetting
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int? ExpireTimeAccessTokenInMinutes { get; set; }
    public long? ExpireTimeRefreshTokenInDays { get; set; }
}