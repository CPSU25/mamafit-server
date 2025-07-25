using FluentValidation;
using MamaFit.BusinessObjects.DTO.AppointmentDto;

namespace MamaFit.Services.Validator.Appointment
{
    public class AppointmentRequestValidator : AbstractValidator<AppointmentRequestDto>
    {
        public AppointmentRequestValidator()
        {

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(x => x.BookingTime)
                .NotEmpty().WithMessage("Booking time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Booking time must be in the future.");
        }
    }
}
