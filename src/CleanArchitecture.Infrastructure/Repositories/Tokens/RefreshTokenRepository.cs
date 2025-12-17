using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Tokens;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories.Tokens;

public class RefreshTokenRepository : BaseRepository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}