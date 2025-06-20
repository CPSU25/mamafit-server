using FluentValidation;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;

namespace MamaFit.Services.Validator.MaternityDressDetail
{
    public class MaternityDressDetailRequestValidator : AbstractValidator<MaternityDressDetailRequestDto>
    {
        public MaternityDressDetailRequestValidator()
        {
            RuleFor(x => x.MaternityDressId)
                .NotEmpty().WithMessage("Maternity Dress ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater.");
        }
    }
}
