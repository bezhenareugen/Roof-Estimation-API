using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RoofEstimation.BLL.Services.MailService.Handlers;
using RoofEstimation.Models.Emails;

namespace RoofEstimation.BLL.Services.MailService;

public class MailService(IConfiguration configuration, EmailHandlerFactory emailHandlerFactory) : IMailService
{
    public async Task SendEmailAsync(SendEmail sendEmailParams)
    {
        var handler = emailHandlerFactory.GetHandler(sendEmailParams.EmailType);
        var email = await handler.CreateEmailAsync(sendEmailParams);
        
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