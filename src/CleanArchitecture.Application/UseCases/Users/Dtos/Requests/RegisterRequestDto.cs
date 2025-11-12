using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Application.Constants;
using CleanArchitecture.Domain.Entities.Identities;
using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Users.Dtos.Requests;

public class RegisterRequestDto : IBaseCreateDto<User>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }

    public User MapToEntity()
    {
        return new User
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            UserName = UserName,
            DateOfBirth = DateOfBirth,
            PhoneNumber = PhoneNumber,
        };
    }
    
    public class Validator : AbstractValidator<RegisterRequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required.");
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(RegexConstants.USERNAME_REGEX_PATTERN)
                .WithMessage("Username must be at least 3 characters long, contain only letters, numbers, '.', '_', or '@', and cannot start or end with '.', '_', or '@'.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(RegexConstants.PHONE_NUMBER_REGEX_PATTERN)
                .WithMessage("Phone number must be a valid Vietnamese number starting with +84 or 0 (e.g. 0912345678 or +84912345678).");
            
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.");
        }
    }
}