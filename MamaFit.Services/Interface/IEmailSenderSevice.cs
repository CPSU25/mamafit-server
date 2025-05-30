namespace MamaFit.Services.Interface;

public interface IEmailSenderSevice
{
    Task SendEmailAsync(string email, string subject, string message);
}