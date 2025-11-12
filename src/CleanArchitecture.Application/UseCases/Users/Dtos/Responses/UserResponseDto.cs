using CleanArchitecture.Application.Common.Dtos;

namespace CleanArchitecture.Application.UseCases.Users.Dtos.Responses;

public class UserResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; }
    public UserInfoDto User { get; set; } = new();
}

public class UserInfoDto : IBaseGetDto<Domain.Entities.Identities.User>
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public void MapFromEntity(Domain.Entities.Identities.User entity)
    {
        Id = entity.Id;
        Email = entity.Email;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
    }
}
