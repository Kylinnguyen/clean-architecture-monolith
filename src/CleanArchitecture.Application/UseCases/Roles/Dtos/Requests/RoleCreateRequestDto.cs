using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Entities.Identities;
using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Roles.Dtos.Requests;

public class RoleCreateRequestDto : IBaseCreateDto<Role>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Role MapToEntity()
    {
        return new Role
        {
            Name = Name,
            Description = Description
        };
    }

    public class Validator : AbstractValidator<RoleCreateRequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
        }
    }
}

