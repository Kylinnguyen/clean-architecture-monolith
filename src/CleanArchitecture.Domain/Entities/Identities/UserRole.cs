using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities.Identities;

public class UserRole : IdentityUserRole<Guid>
{
    public Role Role { get; set; }

    public User User { get; set; }
}