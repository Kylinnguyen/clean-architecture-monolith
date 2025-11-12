using CleanArchitecture.Application.Interfaces.Services.Identity;
using CleanArchitecture.Application.UseCases.Roles.Dtos.Requests;
using CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;
using CleanArchitecture.Domain.Entities.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers;

[Route("api/role")]
[ApiController]
[Authorize]
public class RoleController : BaseApiController<Role, Guid, RoleCreateRequestDto, RoleUpdateRequestDto, RoleDetailDto,
    RoleListDto, RoleSimpleDto>
{
    public RoleController(IRoleService roleService) : base(roleService)
    {
    }
}