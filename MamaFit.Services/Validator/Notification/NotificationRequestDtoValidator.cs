using FluentValidation;
using MamaFit.BusinessObjects.DTO.NotificationDto;

namespace MamaFit.Services.Validator.Notification;

public class NotificationRequestDtoValidator : AbstractValidator<NotificationRequestDto>
{
    public NotificationRequestDtoValidator()
    {
        RuleFor(x => x.NotificationTitle)
            .NotEmpty().WithMessage("Notification title is required.")
            .MaximumLength(100).WithMessage("Notification title must not exceed 100 characters.");

        RuleFor(x => x.NotificationContent)
            .NotEmpty().WithMessage("Notification content is required.")
            .MaximumLength(500).WithMessage("Notification content must not exceed 500 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid notification type.");

        RuleFor(x => x.ActionUrl)
            .MaximumLength(200).WithMessage("Action URL must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.ActionUrl));

        RuleFor(x => x.Metadata)
            .Must(meta => meta == null || System.Text.Json.JsonSerializer.Serialize(meta).Length <= 500)
            .WithMessage("Metadata must not exceed 500 characters when serialized.");

        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("Receiver ID is required.");
    }
}
