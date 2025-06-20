using FluentValidation;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;

namespace MamaFit.Services.Validator.ChatMessage
{
    public class ChatMessageRequestValidator : AbstractValidator<ChatMessageCreateDto>
    {
        public ChatMessageRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(500).WithMessage("Content must not exceed 500 characters.");

            RuleFor(x => x.SenderId)
                .NotEmpty().WithMessage("SenderId is required.");

            RuleFor(x => x.ChatRoomId)
                .NotEmpty().WithMessage("ChatRoomId is required.");
            
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .IsInEnum().WithMessage("Type must be a valid enum value.");
        }
    }
}
