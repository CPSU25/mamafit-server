using FluentValidation;
using MamaFit.BusinessObjects.DTO.MeasurementDto;

namespace MamaFit.Services.Validator.Measurement;

public class MeasurementCreateDtoValidator : AbstractValidator<MeasurementCreateDto>
{
    public MeasurementCreateDtoValidator()
    {
        RuleFor(x => x.MeasurementDiaryId)
            .NotEmpty().WithMessage("Measurement diary ID is required.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.")
            .NotEmpty().WithMessage("Weight is required.");

        RuleFor(x => x.Bust)
            .GreaterThan(0).WithMessage("Bust measurement must be greater than 0.")
            .NotEmpty().WithMessage("Bust measurement is required.");

        RuleFor(x => x.Waist)
            .GreaterThan(0).WithMessage("Waist measurement must be greater than 0.")
            .NotEmpty().WithMessage("Waist measurement is required.");

        RuleFor(x => x.Hip)
            .GreaterThan(0).WithMessage("Hip measurement must be greater than 0.")
            .NotEmpty().WithMessage("Hip measurement is required.");
    }
}