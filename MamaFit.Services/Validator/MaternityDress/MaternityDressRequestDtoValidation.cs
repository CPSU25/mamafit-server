using FluentValidation;
using MamaFit.BusinessObjects.DTO.MaternityDressDto;

namespace MamaFit.Services.Validator.MaternityDress
{
    public class MaternityDressRequestDtoValidation : AbstractValidator<MaternityDressRequestDto>
    {
        public MaternityDressRequestDtoValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(100).WithMessage("Slug must not exceed 100 characters.");
        }
    }
}
