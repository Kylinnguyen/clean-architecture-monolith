using CleanArchitecture.Infrastructure.Persistence.EntityConfigurations.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common.Core.BaseEntities;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim,
    UserRole, UserLogin, RoleClaim, UserToken>
{
    private readonly ICurrentUser _currentUser;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureCoreIdentityEntities();
        // Apply all configurations
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IFullAuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUser.UserId;
                    entry.Entity.CreatedTime = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUser.UserId;
                    entry.Entity.LastModifiedTime = DateTimeOffset.UtcNow;
                    break;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}