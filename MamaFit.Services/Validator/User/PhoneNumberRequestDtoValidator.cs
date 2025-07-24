using FluentValidation;
using MamaFit.BusinessObjects.DTO.AuthDto;

namespace MamaFit.Services.Validator.User;

public class PhoneNumberRequestDtoValidator : AbstractValidator<PhoneNumberRequestDto>
{
    public PhoneNumberRequestDtoValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^0\d{9}$").WithMessage("Invalid phone number format.");
    }
}