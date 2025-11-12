using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UnitOfWork<TGenericDbContext> : IUnitOfWork where TGenericDbContext : DbContext
{
    private readonly TGenericDbContext _genericDbContext;
    private IDbContextTransaction? _dbContextTransaction;
    private bool _disposed;

    public UnitOfWork(TGenericDbContext genericDbContext)
    {
        _genericDbContext = genericDbContext;
        _dbContextTransaction = null;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _genericDbContext.Dispose();
        _disposed = true;
    }

    public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        await _genericDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContextTransaction != null) return;
        _dbContextTransaction = await _genericDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContextTransaction != null) await _dbContextTransaction.RollbackAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _genericDbContext.SaveChangesAsync(cancellationToken);
            await _dbContextTransaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }
}