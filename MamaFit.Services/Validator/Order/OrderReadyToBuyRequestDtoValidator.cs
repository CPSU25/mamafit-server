using FluentValidation;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.Services.Validator.OrderItem;

namespace MamaFit.Services.Validator.Order
{
    public class OrderReadyToBuyRequestDtoValidator : AbstractValidator<OrderReadyToBuyRequestDto>
    {
        public OrderReadyToBuyRequestDtoValidator()
        {
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("PaymentMethod must be a valid enum value.");

            RuleFor(x => x.DeliveryMethod)
                .IsInEnum().WithMessage("DeliveryMethod must be a valid enum value.");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new OrderItemReadyToBuyRequestValidator());
        }
    }
}
