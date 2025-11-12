
using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Entities.Identities;

namespace CleanArchitecture.Application.UseCases.Roles.Dtos.Responses;

public class RoleDetailDto : IBaseGetDto<Role>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedTime { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }

    public void MapFromEntity(Role entity)
    {
        Id = entity.Id;
        Name = entity.Name ?? string.Empty;
        Description = entity.Description;
        CreatedBy = entity.CreatedBy;
        CreatedTime = entity.CreatedTime;
        LastModifiedBy = entity.LastModifiedBy;
        LastModifiedTime = entity.LastModifiedTime;
    }
}
