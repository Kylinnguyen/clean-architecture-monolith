using CleanArchitecture.Domain.Common.Core.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities.Identities;

public class User : IdentityUser<Guid>, IFullAuditableEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }

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