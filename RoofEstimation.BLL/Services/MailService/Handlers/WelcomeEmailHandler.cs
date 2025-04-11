using Microsoft.Extensions.Configuration;
using MimeKit;
using RoofEstimation.BLL.MailTemplates;
using RoofEstimation.Models.Emails;

namespace RoofEstimation.BLL.Services.MailService.Handlers;

public class WelcomeEmailHandler(IConfiguration configuration) : IEmailHandler
{
    public Task<MimeMessage> CreateEmailAsync(SendEmail sendEmailParams)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Roof Estimation", configuration["EmailSettings:From"]));
        email.To.Add(new MailboxAddress(sendEmailParams.ToName, sendEmailParams.ToEmailAddress));
        email.Subject = sendEmailParams.Subject;

        var body = EmailTemplates.NewWelcomeEmail;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body.Replace("{{Name}}", sendEmailParams.ToName),
        };
        email.Body = bodyBuilder.ToMessageBody();
        
        return Task.FromResult(email);
    }
}