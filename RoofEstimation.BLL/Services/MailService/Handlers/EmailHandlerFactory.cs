using Microsoft.Extensions.DependencyInjection;
using RoofEstimation.Models.Enums;

namespace RoofEstimation.BLL.Services.MailService.Handlers;

public class EmailHandlerFactory(IServiceProvider serviceProvider)
{
    public IEmailHandler GetHandler(EmailType emailType)
    {
        IEmailHandler test;
        switch (emailType)
        {
            case EmailType.WelcomeEmail:
                test = serviceProvider.GetRequiredService<WelcomeEmailHandler>();
                break;
            case EmailType.ResetPasswordEmail:
                test = serviceProvider.GetRequiredService<ResetPasswordEmilHandler>();
                break;
            default:
                throw new ArgumentException("Invalid email type", nameof(emailType));
        }

        return test;
    }
}