using FluentValidation;
using MamaFit.BusinessObjects.DTO.VoucherBatchDto;

namespace MamaFit.Services.Validator.VoucherBatch;

public class VoucherBatchDtoValidator : AbstractValidator<VoucherBatchRequestDto>
{
    public VoucherBatchDtoValidator()
    {
        RuleFor(x => x.BatchName)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.TotalQuantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).WithMessage("DiscountValue must be an positive interger");
    }
}