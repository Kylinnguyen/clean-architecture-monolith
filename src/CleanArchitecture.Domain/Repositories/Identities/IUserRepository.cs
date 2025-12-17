using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Domain.Repositories.Identities;

public interface IUserRepository : IBaseRepository<User, Guid>
{
}