using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Domain.Common.Core.BaseEntities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Domain.Repositories.Identity;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Repositories.Identity;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class RepositoryCollectionExtensions
{
    public static void AddRepositoriesConfiguration(this IServiceCollection services)
    {
        // Register generic repositories
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

        // Register specific repositories
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
    }
}