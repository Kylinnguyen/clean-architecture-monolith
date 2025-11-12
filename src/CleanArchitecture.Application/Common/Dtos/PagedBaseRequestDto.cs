using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace CleanArchitecture.Application.Common.Dtos;

public class PagedBaseRequestDto
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    
    public class Validator : AbstractValidator<PagedBaseRequestDto>
    {
        public Validator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}

