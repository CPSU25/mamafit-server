using FluentValidation;
using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;

namespace MamaFit.Services.Validator.VoucherDiscount;

public class VoucherDiscountRequestDtoValidator : AbstractValidator<VoucherDiscountRequestDto>
{
    public VoucherDiscountRequestDtoValidator()
    {
        RuleFor(x => x.VoucherBatchId)
            .NotEmpty().WithMessage("VoucherBatchId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status is invalid.");
    }
}