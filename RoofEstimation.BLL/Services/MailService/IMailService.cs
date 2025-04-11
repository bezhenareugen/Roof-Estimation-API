using RoofEstimation.Models.Emails;

namespace RoofEstimation.BLL.Services.MailService;

public interface IMailService
{
    Task SendEmailAsync(SendEmail sendEmailParams);
}