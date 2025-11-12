using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;

public class RoleSimpleDto : IBaseGetDto<Role>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public void MapFromEntity(Role entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
    }
}

