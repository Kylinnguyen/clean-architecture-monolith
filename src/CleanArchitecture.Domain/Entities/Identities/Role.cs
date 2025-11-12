using CleanArchitecture.Domain.Common.Core.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities.Identities;

public class Role : IdentityRole<Guid>, IFullAuditableEntity<Guid>
{
    public const string Admin = "Admin";
    public const string User = "User";
    
    
    public string Description { get; set; }
    
    #region References

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    #endregion

    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public object?[] GetKeys()
    {
        return [Id];
    }
}