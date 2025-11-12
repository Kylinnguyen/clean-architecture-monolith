using System.Linq.Expressions;
using CleanArchitecture.Domain.Common.Core.BaseEntities;
using CleanArchitecture.Application.Common.Dtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IBaseService<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    // where TCreateRequestDto : class
    // where TUpdateRequestDto : class
    // where TDetailDto : class
    // where TListDto : class
    // where TSimpleDto : class
{
    Task<TDetailDto> GetDetailAsync(TKey id, CancellationToken cancellationToken = default);
    Task<List<TListDto>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get paginated list of entities with optional filtering
    /// </summary>
    Task<PagedResponseDto<TListDto>> GetPagedListAsync(
        PagedBaseRequestDto baseRequest,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<TSimpleDto> CreateAsync(TCreateRequestDto request, CancellationToken cancellationToken = default);
    Task<TSimpleDto> UpdateAsync(TUpdateRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}