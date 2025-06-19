using FluentValidation;
using MamaFit.BusinessObjects.DTO.MaternityDressTask;

namespace MamaFit.Services.Validator.MaternityDressTask
{
    public class MaternityDressTaskRequestValidator : AbstractValidator<MaternityDressTaskRequestDto>
    {
        public MaternityDressTaskRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .IsInEnum().WithMessage("Type must be a valid enum value.");
        }
    }
}
