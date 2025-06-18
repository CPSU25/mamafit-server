using FluentValidation;
using MamaFit.BusinessObjects.DTO.MeasurementDto;

namespace MamaFit.Services.Validator.MeasurementDiary;

public class MeasurementDiaryDtoValidator : AbstractValidator<MeasurementDiaryDto>
{
    public MeasurementDiaryDtoValidator()
    {
        RuleFor(x => x.UltrasoundDate)
            .NotNull()
            .When(x => x.WeeksFromUltrasound > 0)
            .WithMessage("Ultrasound date is required when weeks from ultrasound is provided.");
        
        RuleFor(x => x.WeeksFromUltrasound)
            .GreaterThan(0)
            .When(x => x.UltrasoundDate.HasValue)
            .WithMessage("Weeks from ultrasound must be greater than 0 when ultrasound date is provided.");
        
        RuleFor(x => x.WeeksFromUltrasound)
            .Equal(0)
            .When(x => !x.UltrasoundDate.HasValue)
            .WithMessage("Weeks from ultrasound must not be provided when ultrasound date is missing.");
        
        RuleFor(x => x.FirstDateOfLastPeriod)
            .LessThan(x => x.UltrasoundDate)
            .When(x => x.FirstDateOfLastPeriod.HasValue && x.UltrasoundDate.HasValue)
            .WithMessage("First date of last period must be earlier than ultrasound date.");
        
        RuleFor(x => x.AverageMenstrualCycle)
            .InclusiveBetween(21, 35)
            .When(x => x.AverageMenstrualCycle.HasValue)
            .WithMessage("Average menstrual cycle should be between 21 and 35 days.");
        
        RuleFor(x => x.FirstDateOfLastPeriod)
            .NotEmpty()
            .When(x => !x.UltrasoundDate.HasValue && x.WeeksFromUltrasound == 0)
            .WithMessage("Either the first date of last period or ultrasound information is required to calculate pregnancy start date.");

        // Composite rule: If both ultrasound info and period info are provided, check for logic consistency
        RuleFor(x => x)
            .Custom((dto, context) =>
            {
                if (dto.UltrasoundDate.HasValue && dto.WeeksFromUltrasound > 0 && dto.FirstDateOfLastPeriod.HasValue)
                {
                    var startByUS = dto.UltrasoundDate.Value.AddDays(-7 * dto.WeeksFromUltrasound);
                    var cycle = dto.AverageMenstrualCycle ?? 28;
                    var startByFOLP = dto.FirstDateOfLastPeriod.Value.AddDays(cycle - 14);

                    var daysDiff = Math.Abs((startByUS - startByFOLP).TotalDays);
                    if (daysDiff > 21)
                    {
                        context.AddFailure("There is a logical inconsistency between ultrasound and period information (pregnancy start dates differ by more than 3 weeks). Please check your input.");
                    }
                }
            });
    }
}
