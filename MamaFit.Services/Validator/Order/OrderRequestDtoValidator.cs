using FluentValidation;
using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.Services.Validator.Order;

public class OrderRequestDtoValidator : AbstractValidator<OrderRequestDto>
{
    public OrderRequestDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Order type is required.")
            .IsInEnum().WithMessage("Order type is invalid.");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Order status is required.")
            .IsInEnum().WithMessage("Order status is invalid.");
        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        RuleFor(x => x.ShippingFee)
            .GreaterThan(0).WithMessage("Shipping fee must be greater than zero.");
        RuleFor(x => x.PaymentStatus)
            .NotEmpty().WithMessage("Payment status is required.")
            .IsInEnum().WithMessage("Payment status is invalid.");
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .IsInEnum().WithMessage("Payment method is invalid.");
        RuleFor(x => x.DeliveryMethod)
            .NotEmpty().WithMessage("Delivery method is required.")
            .IsInEnum().WithMessage("Delivery method is invalid.");
        RuleFor(x => x.PaymentType)
            .NotEmpty().WithMessage("Payment type is required.")
            .IsInEnum().WithMessage("Payment type is invalid.");
        RuleFor(x => x.SubTotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Sub total amount must be greater than or equal to zero.");
    }
}