using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Domain.Repositories.Identity;

public interface IUserRepository : IBaseRepository<User, Guid>
{
}