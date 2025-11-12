using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;

public class RoleListDto : IBaseGetListDto<Role>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public void MapFromEntity(Role entity)
    {
        Id = entity.Id;
        Name = entity.Name ?? string.Empty;
        Description = entity.Description;
    }
}
