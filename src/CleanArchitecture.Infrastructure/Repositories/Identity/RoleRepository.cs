using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identity;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories.Identity;

public class RoleRepository : BaseRepository<Role, Guid>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

