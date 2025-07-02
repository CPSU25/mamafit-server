using FluentValidation;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

namespace MamaFit.Services.Validator.WarrantyRequest
{
    public class WarrantyRequestCreateValidator : AbstractValidator<WarrantyRequestCreateDto>
    {
        public WarrantyRequestCreateValidator()
        {
            RuleFor(x => x.OriginalOrderItemId)
                .NotEmpty()
                .WithMessage("OriginalOrderItemId is required.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid enum value.");
        }
    }
}
