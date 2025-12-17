using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Domain.Repositories.Tokens;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, Guid>
{
    
}