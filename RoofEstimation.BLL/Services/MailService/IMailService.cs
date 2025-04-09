using RoofEstimation.Entities.Auth;

namespace RoofEstimation.BLL.Services.MailService;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, UserEntity user, byte[]? attachment);
}