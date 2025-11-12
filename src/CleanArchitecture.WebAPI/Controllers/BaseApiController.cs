using System.Net;
using CleanArchitecture.WebAPI.Common;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Common.Core.BaseEntities;

namespace CleanArchitecture.WebAPI.Controllers;

[ApiController]
// [Route("api/[controller]")]
public abstract class BaseApiController<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto> : ControllerBase
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TCreateRequestDto : class
    where TUpdateRequestDto : class
    where TDetailDto : class, IBaseGetDto<TEntity>
    where TListDto : class, IBaseGetListDto<TEntity>
    where TSimpleDto : class, IBaseGetDto<TEntity>
{
    private readonly IBaseService<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto> _baseService;

    protected BaseApiController(IBaseService<TEntity, TKey, TCreateRequestDto, TUpdateRequestDto, TDetailDto, TListDto, TSimpleDto> baseService)
    {
        _baseService = baseService;
    }


    [HttpGet("{id}")]
    public virtual async Task<ApiResult<TDetailDto>> GetDetailAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var dto = await _baseService.GetDetailAsync(id, cancellationToken);
        return ApiResult<TDetailDto>.Ok(dto);
    }

    [HttpGet]
    public virtual async Task<ApiResult<List<TListDto>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _baseService.GetListAsync(cancellationToken: cancellationToken);
        return ApiResult<List<TListDto>>.Ok(result);
    }

    /// <summary>
    /// Get paginated list of entities
    /// </summary>
    [HttpGet("paged")]
    public virtual async Task<ApiResult<PagedResponseDto<TListDto>>> GetPagedListAsync(
        [FromQuery] PagedBaseRequestDto baseRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _baseService.GetPagedListAsync(baseRequest, cancellationToken: cancellationToken);
        return ApiResult<PagedResponseDto<TListDto>>.Ok(result);
    }

    [HttpPost]
    public virtual async Task<ApiResult<TSimpleDto>> CreateAsync([FromBody] TCreateRequestDto request, CancellationToken cancellationToken = default)
    {
        var dto = await _baseService.CreateAsync(request, cancellationToken);
        return ApiResult<TSimpleDto>.Ok(dto);
    }

    [HttpPut]
    public virtual async Task<ApiResult<TSimpleDto>> UpdateAsync([FromBody] TUpdateRequestDto request, CancellationToken cancellationToken = default)
    {
        var dto = await _baseService.UpdateAsync(request, cancellationToken);
        return ApiResult<TSimpleDto>.Ok(dto);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ApiResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var result = await _baseService.DeleteAsync(id, cancellationToken);
        return result ? Common.ApiResult.Ok() : Common.ApiResult.Fail();
    }
}
