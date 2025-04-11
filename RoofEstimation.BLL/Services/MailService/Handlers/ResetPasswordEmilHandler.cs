using Microsoft.Extensions.Configuration;
using MimeKit;
using RoofEstimation.Models.Emails;

namespace RoofEstimation.BLL.Services.MailService.Handlers;

public class ResetPasswordEmilHandler(IConfiguration configuration) : IEmailHandler
{
    public Task<MimeMessage> CreateEmailAsync(SendEmail sendEmailParams)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Roof Estimation", configuration["EmailSettings:From"]));
        email.To.Add(new MailboxAddress(sendEmailParams.ToName, sendEmailParams.ToEmailAddress));
        email.Subject = sendEmailParams.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = sendEmailParams.Body,
        };
        email.Body = bodyBuilder.ToMessageBody();
        
        return Task.FromResult(email);
    }
}