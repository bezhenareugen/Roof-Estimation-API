using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RoofEstimation.BLL.MailTemplates;
using RoofEstimation.Entities.Auth;

namespace RoofEstimation.BLL.Services.MailService;

public class MailService(IConfiguration configuration) : IMailService
{

    public async Task SendWelcomeEmail()
    {
        
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body, UserEntity user, byte[]? attachment)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Roof Estimation", configuration["EmailSettings:From"]));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;
        
        var htmlBody = EmailTemplates.NewWelcomeEmail.Replace("{{Name}}", $"{user.FirstName} {user.LastName}");

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };
        email.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(configuration["EmailSettings:SmtpServer"],
                465);
            await smtpClient.AuthenticateAsync(configuration["EmailSettings:Username"],
                configuration["EmailSettings:Password"]);
            await smtpClient.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
}