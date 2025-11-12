using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Domain.Common.Core.BaseEntities;
using System.Linq.Expressions;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        return Task.FromResult(_dbContext.Set<TEntity>().AsQueryable());
    }

    public async Task<List<TEntity>> GetListAsync(bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<(List<TEntity> Items, int TotalCount)> GetPagedListAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        // Apply predicate if provided
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<TEntity> AddAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var addedEntity = await _dbSet.AddAsync(entity, cancellationToken);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);

        return addedEntity.Entity;
    }

    public async Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var updatedEntity = _dbSet.Update(entity);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);

        return updatedEntity.Entity;
    }

    public async Task RemoveAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class BaseRepository<TEntity, TKey> : BaseRepository<TEntity>, IBaseRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetAsync(TKey id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException(nameof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity?> FindAsync(TKey id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public async Task<bool> RemoveAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
        if (entity == null) return false;
        _dbContext.Set<TEntity>().Remove(entity);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task RemoveRangeAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var removedEntities = await _dbContext.Set<TEntity>()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        if (!removedEntities.Any()) return;
        _dbContext.Set<TEntity>().RemoveRange(removedEntities);
        if (autoSave) await _dbContext.SaveChangesAsync(cancellationToken);
    }
}