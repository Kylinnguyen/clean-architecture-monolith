using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identity;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories.Identity;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}