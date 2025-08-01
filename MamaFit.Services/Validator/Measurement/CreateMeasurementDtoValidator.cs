using FluentValidation;
using MamaFit.BusinessObjects.DTO.MeasurementDto;

namespace MamaFit.Services.Validator.Measurement;

public class CreateMeasurementDtoValidator : AbstractValidator<CreateMeasurementDto>
{
    public CreateMeasurementDtoValidator()
    {
        RuleFor(x => x.MeasurementId)
            .NotEmpty().WithMessage("Measurement diary ID is required.");
        RuleFor(x => x.WeekOfPregnancy)
            .GreaterThan(0).WithMessage("Week of pregnancy must be greater than 0.")
            .NotEmpty().WithMessage("Week of pregnancy is required.");
        RuleFor(x => x.Neck)
            .GreaterThan(0).WithMessage("Neck measurement must be greater than 0.")
            .NotEmpty().WithMessage("Neck measurement is required.");
        RuleFor(x => x.Coat)
            .GreaterThan(0).WithMessage("Coat measurement must be greater than 0.")
            .NotEmpty().WithMessage("Coat measurement is required.");
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.")
            .NotEmpty().WithMessage("Weight is required.");
        RuleFor(x => x.Bust)
            .GreaterThan(0).WithMessage("Bust measurement must be greater than 0.")
            .NotEmpty().WithMessage("Bust measurement is required.");
        RuleFor(x => x.ChestAround)
            .GreaterThan(0).WithMessage("Chest around measurement must be greater than 0.")
            .NotEmpty().WithMessage("Chest around measurement is required.");
        RuleFor(x => x.Stomach)
            .GreaterThan(0).WithMessage("Stomach measurement must be greater than 0.")
            .NotEmpty().WithMessage("Stomach measurement is required.");
        RuleFor(x => x.PantsWaist)
            .GreaterThan(0).WithMessage("Pants waist measurement must be greater than 0.")
            .NotEmpty().WithMessage("Pants waist measurement is required.");
        RuleFor(x => x.Thigh)
            .GreaterThan(0).WithMessage("Thigh measurement must be greater than 0.")
            .NotEmpty().WithMessage("Thigh measurement is required.");
        RuleFor(x => x.DressLength)
            .GreaterThan(0).WithMessage("Dress length measurement must be greater than 0.")
            .NotEmpty().WithMessage("Dress length measurement is required.");
        RuleFor(x => x.SleeveLength)
            .GreaterThan(0).WithMessage("Sleeve length measurement must be greater than 0.")
            .NotEmpty().WithMessage("Sleeve length measurement is required.");
        RuleFor(x => x.ShoulderWidth)
            .GreaterThan(0).WithMessage("Shoulder width measurement must be greater than 0.")
            .NotEmpty().WithMessage("Shoulder width measurement is required.");
        RuleFor(x => x.Waist)
            .GreaterThan(0).WithMessage("Waist measurement must be greater than 0.")
            .NotEmpty().WithMessage("Waist measurement is required.");
        RuleFor(x => x.LegLength)
            .GreaterThan(0).WithMessage("Leg length measurement must be greater than 0.")
            .NotEmpty().WithMessage("Leg length measurement is required.");
        RuleFor(x => x.Hip)
            .GreaterThan(0).WithMessage("Hip measurement must be greater than 0.")
            .NotEmpty().WithMessage("Hip measurement is required.");
    }
}