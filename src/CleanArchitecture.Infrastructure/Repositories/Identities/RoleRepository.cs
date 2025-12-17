using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identities;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories.Identities;

public class RoleRepository : BaseRepository<Role, Guid>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

