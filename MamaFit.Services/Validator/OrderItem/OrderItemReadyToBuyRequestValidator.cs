using FluentValidation;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.Services.Validator.OrderItem
{
    public class OrderItemReadyToBuyRequestValidator : AbstractValidator<OrderItemReadyToBuyRequestDto>
    {
        public OrderItemReadyToBuyRequestValidator()
        {
            RuleFor(x => x.MaternityDressDetailId)
                .NotNull().WithMessage("MaternityDressDetailId can not be null")
                .NotEmpty().WithMessage("MaternityDressDetailId can not be null");

            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Quantity can not be null");
        }
    }
}
