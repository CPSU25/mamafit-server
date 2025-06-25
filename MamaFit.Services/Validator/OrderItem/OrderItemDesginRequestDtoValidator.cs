using FluentValidation;
using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.Services.Validator.OrderItem
{
    public class OrderItemDesginRequestDtoValidator : AbstractValidator<OrderDesignRequestDto>
    {
        public OrderItemDesginRequestDtoValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");
        }
    }
}
