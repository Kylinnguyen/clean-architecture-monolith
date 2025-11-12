using CleanArchitecture.Application.UseCases.Roles.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Application.Interfaces.Services.Identity;

public interface IRoleService : IBaseService<Role, Guid, RoleCreateRequestDto, RoleUpdateRequestDto, RoleDetailDto,
    RoleListDto, RoleSimpleDto>
{
}