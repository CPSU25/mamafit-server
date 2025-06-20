using FluentValidation;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;

namespace MamaFit.Services.Validator.DesignRequest
{
    public class DesginRequestValidator : AbstractValidator<DesignRequestCreateDto>
    {
        public DesginRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.OrderItemId)
                .NotEmpty()
                .WithMessage("OrderItemId is required.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
