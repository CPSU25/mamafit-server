using FluentValidation;
using MamaFit.BusinessObjects.DTO.RoleDto;

namespace MamaFit.Services.Validator.Role;

public class RoleRequestDtoValidator : AbstractValidator<RoleRequestDto>
{
    public RoleRequestDtoValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
    }
}