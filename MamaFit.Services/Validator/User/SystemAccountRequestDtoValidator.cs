using FluentValidation;
using MamaFit.BusinessObjects.DTO.AuthDto;

namespace MamaFit.Services.Validator.User;

public class SystemAccountRequestDtoValidator : AbstractValidator<SystemAccountRequestDto>
{
    public SystemAccountRequestDtoValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^0\d{9}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage("Password must contain at least one special character.");
    }
}