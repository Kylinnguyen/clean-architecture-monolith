using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Common.Core.BaseEntities;

namespace CleanArchitecture.Domain.Repositories;

/// <summary>
/// Base repository interface providing comprehensive CRUD operations
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public interface IBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    #region Queryable Support

    Task<IQueryable<TEntity>> GetQueryableAsync();

    #endregion

    #region Query methods

    Task<List<TEntity>> GetListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get paginated list of entities with optional filtering
    /// </summary>
    /// <param name="pageNumber">Page number (1-based index)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="predicate">Optional filter predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple containing items and total count</returns>
    Task<(List<TEntity> Items, int TotalCount)> GetPagedListAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region CRUD Operations

    Task<TEntity> AddAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task RemoveAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    #endregion

    #region Bulk Operations

    Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    #endregion
}

public interface IBaseRepository<TEntity, in TKey> : IBaseRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);
}