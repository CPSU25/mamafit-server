using FluentValidation;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;

namespace MamaFit.Services.Validator.ComponentOption
{
    public class ComponentOptionRequestValidator : AbstractValidator<ComponentOptionRequestDto>
    {
        public ComponentOptionRequestValidator()
        {
            RuleFor(x => x.ComponentId)
                .NotEmpty().WithMessage("Component ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Component option name is required.")
                .MaximumLength(100).WithMessage("Component option name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        }
    }
}
