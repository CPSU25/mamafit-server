using FluentValidation;
using MamaFit.BusinessObjects.DTO.ComponentDto;

namespace MamaFit.Services.Validator.Component
{
    public class ComponentRequestValidator : AbstractValidator<ComponentRequestDto>
    {
        public ComponentRequestValidator()
        {
            RuleFor(x => x.StyleId)
                .NotEmpty().WithMessage("Component ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Component name is required.")
                .MaximumLength(100).WithMessage("Component name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
