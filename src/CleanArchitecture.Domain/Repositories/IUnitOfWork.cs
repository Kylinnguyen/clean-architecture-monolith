using CleanArchitecture.Domain.Common.Core.BaseEntities;

namespace CleanArchitecture.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangeAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}