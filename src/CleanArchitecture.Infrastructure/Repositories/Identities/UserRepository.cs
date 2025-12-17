using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identities;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories.Identities;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}