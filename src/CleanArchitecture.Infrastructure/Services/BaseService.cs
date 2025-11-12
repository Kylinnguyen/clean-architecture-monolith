using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Common.Core.BaseEntities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.Services;

public abstract class BaseService<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto>
    : IBaseService<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TCreateRequestDto : class, IBaseCreateDto<TEntity>
    where TUpdateRequestDto : class, IBaseUpdateDto<TEntity, TKey>
    where TDetailDto : class, IBaseGetDto<TEntity>, new()
    where TListDto : class, IBaseGetListDto<TEntity>, new()
    where TSimpleDto : class, IBaseGetDto<TEntity>, new()
{
    private readonly IBaseRepository<TEntity, TKey> _baseRepository;

    protected BaseService(IBaseRepository<TEntity, TKey> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public virtual async Task<TDetailDto> GetDetailAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _baseRepository.GetAsync(id, cancellationToken: cancellationToken);
        if (entity == null) throw new NotFoundException(nameof(TEntity), id);
        var detailDto = new TDetailDto();
        detailDto.MapFromEntity(entity);
        return detailDto;
    }

    public virtual async Task<List<TListDto>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            predicate = x => true;
        }
        var query = await _baseRepository.GetQueryableAsync();
        var entities = await query.Where(predicate).ToListAsync(cancellationToken);
        var results = entities.Select(x =>
        {
            var dto = new TListDto();
            dto.MapFromEntity(x);
            return dto;
        }).ToList();

        return results;
    }

    public virtual async Task<PagedResponseDto<TListDto>> GetPagedListAsync(
        PagedBaseRequestDto baseRequest,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        // Get paginated entities from repository
        var (entities, totalCount) = await _baseRepository.GetPagedListAsync(
            baseRequest.PageNumber,
            baseRequest.PageSize,
            predicate,
            cancellationToken);

        // Map entities to DTOs
        var items = entities.Select(x =>
        {
            var dto = new TListDto();
            dto.MapFromEntity(x);
            return dto;
        }).ToList();

        // Create and return paginated response
        return new PagedResponseDto<TListDto>(items, totalCount, baseRequest.PageNumber, baseRequest.PageSize);
    }

    public virtual async Task<TSimpleDto> CreateAsync(TCreateRequestDto request, CancellationToken cancellationToken = default)
    {
        // TODO: Add fluent validation for validate then
        var newEntity = request.MapToEntity();
        await _baseRepository.AddAsync(newEntity, true, cancellationToken);
        var result = new TSimpleDto();
        result.MapFromEntity(newEntity);
        return result;
    }

    public virtual async Task<TSimpleDto> UpdateAsync(TUpdateRequestDto request, CancellationToken cancellationToken = default)
    {
        // TODO: Add fluent validation for validate then
        
        var queryable = await _baseRepository.GetQueryableAsync();
        var entity = await queryable
            .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken)
            ?? throw new NotFoundException(nameof(TEntity), request.Id);

        var newEntity = request.MapToEntity(entity);
        await _baseRepository.UpdateAsync(newEntity, true, cancellationToken);
        var result = new TSimpleDto();
        result.MapFromEntity(newEntity);
        return result;
    }

    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _baseRepository.FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null) throw new NotFoundException(nameof(TEntity), id);
        await _baseRepository.RemoveAsync(entity, true, cancellationToken);

        return true;
    }
}