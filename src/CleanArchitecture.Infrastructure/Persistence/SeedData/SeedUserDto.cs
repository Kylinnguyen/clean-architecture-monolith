namespace CleanArchitecture.Infrastructure.Persistence.SeedData;

public class SeedUserDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Role { get; set; }
}