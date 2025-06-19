using FluentValidation;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;

namespace MamaFit.Services.Validator.ChatRoom
{
    public class ChatRoomRequestValidator : AbstractValidator<ChatRoomCreateDto>
    {
        public ChatRoomRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Chat room name is required.")
                .MaximumLength(100).WithMessage("Chat room name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
