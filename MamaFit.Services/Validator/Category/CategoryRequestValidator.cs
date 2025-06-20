using FluentValidation;
using MamaFit.BusinessObjects.DTO.CategoryDto;

namespace MamaFit.Services.Validator.Category
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
