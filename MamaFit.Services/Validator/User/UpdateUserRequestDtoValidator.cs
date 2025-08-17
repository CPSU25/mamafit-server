using FluentValidation;
using MamaFit.BusinessObjects.DTO.UserDto;

namespace MamaFit.Services.Validator.User;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator()
    {
        
    }
}