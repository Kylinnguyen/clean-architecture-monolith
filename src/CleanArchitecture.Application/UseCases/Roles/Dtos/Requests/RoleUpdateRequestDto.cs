using CleanArchitecture.Application.Common.Dtos;
using CleanArchitecture.Domain.Entities.Identities;
using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Roles.Dtos.Requests;

public class RoleUpdateRequestDto : IBaseUpdateDto<Role, Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Role MapToEntity(Role entity)
    {
        entity.Name = Name;
        entity.Description = Description;
        return entity;
    }

    public class Validator : AbstractValidator<RoleUpdateRequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Role ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required.");
        }
    }
}
