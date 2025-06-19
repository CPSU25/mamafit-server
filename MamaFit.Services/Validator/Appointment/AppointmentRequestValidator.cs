using FluentValidation;
using MamaFit.BusinessObjects.DTO.AppointmentDto;

namespace MamaFit.Services.Validator.Appointment
{
    public class AppointmentRequestValidator : AbstractValidator<AppointmentRequestDto>
    {
        public AppointmentRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(x => x.BookingTime)
                .NotEmpty().WithMessage("Booking time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Booking time must be in the future.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid enum value.");
        }
    }
}
