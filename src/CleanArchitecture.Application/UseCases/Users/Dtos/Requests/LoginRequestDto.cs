using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Users.Dtos.Requests;

public class LoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    // public bool RememberMe { get; set; } = false;
    
    public class Validator : AbstractValidator<LoginRequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
