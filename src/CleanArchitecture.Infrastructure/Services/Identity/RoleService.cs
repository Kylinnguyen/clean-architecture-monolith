using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces.Services.Identity;
using CleanArchitecture.Application.UseCases.Roles.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;
using CleanArchitecture.Domain.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Services.Identity;

public class RoleService : BaseService<Role, Guid, RoleCreateRequestDto, RoleUpdateRequestDto, RoleDetailDto,
    RoleListDto, RoleSimpleDto>, IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository, RoleManager<Role> roleManager) : base(roleRepository)
    {
        _roleRepository = roleRepository;
        _roleManager = roleManager;
    }

    public override async Task<RoleSimpleDto> CreateAsync(RoleCreateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var newEntity = request.MapToEntity();
        var identityResult = await _roleManager.CreateAsync(newEntity);
        if (!identityResult.Succeeded)
        {
            throw new BusinessException("Error creating role");
        }
        var result = new RoleSimpleDto();
        result.MapFromEntity(newEntity);
        return result;
    }

    public override async Task<RoleSimpleDto> UpdateAsync(RoleUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _roleManager.FindByIdAsync(request.Id.ToString())
            ?? throw new NotFoundException(nameof(Role), request.Id);
        
        var updateEntity = request.MapToEntity(entity);
        var identityResult = await _roleManager.UpdateAsync(updateEntity);
        if (!identityResult.Succeeded)
        {
            throw new BusinessException("Error updating role");
        }
        var result = new RoleSimpleDto();
        result.MapFromEntity(updateEntity);
        return result;
    }
}