using FluentValidation;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.Services.Validator.OrderItem;

public class OrderItemRequestDtoValidator : AbstractValidator<OrderItemRequestDto>
{
    public OrderItemRequestDtoValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.")
            .NotNull().WithMessage("OrderId cannot be null.");
        RuleFor(x => x.MaternityDressDetailId)
            .NotEmpty().WithMessage("MaternityDressDetailId is required.")
            .NotNull().WithMessage("MaternityDressDetailId cannot be null.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
            .NotNull().WithMessage("Quantity cannot be null.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.")
            .NotNull().WithMessage("Price cannot be null.");
    }
}