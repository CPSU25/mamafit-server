using FluentValidation;
using MamaFit.BusinessObjects.DTO.UserDto;

namespace MamaFit.Services.Validator.User;

public class SendOtpDtoValidator : AbstractValidator<SendOTPRequestDto>
{
    public SendOtpDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^0\d{9}$").WithMessage("Invalid phone number format.");
    }
}