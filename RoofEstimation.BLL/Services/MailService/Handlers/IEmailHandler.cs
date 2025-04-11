using MimeKit;
using RoofEstimation.Models.Emails;

namespace RoofEstimation.BLL.Services.MailService.Handlers;

public interface IEmailHandler
{
    Task<MimeMessage> CreateEmailAsync(SendEmail sendEmailParams);
}