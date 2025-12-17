
using CleanArchitecture.Domain.Common.Core.BaseEntities;

namespace CleanArchitecture.Domain.Entities.Identities;

public class RefreshToken : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string TokenHash { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    
    public DateTimeOffset ExpiredTime { get; set; }
    public bool IsExpired => DateTime.Now >= ExpiredTime;
    public DateTimeOffset? RevokedTime { get; set; }
    public bool IsRevoked => RevokedTime != null;
}