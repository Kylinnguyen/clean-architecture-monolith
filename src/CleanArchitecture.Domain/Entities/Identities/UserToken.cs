using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities.Identities;

public class UserToken : IdentityUserToken<Guid>
{
}