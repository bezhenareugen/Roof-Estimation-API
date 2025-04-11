using RoofEstimation.Models.Enums;

namespace RoofEstimation.Models.Emails;

public class SendEmail
{
    public string ToName { get; set; }
    public string ToEmailAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public byte[]? Attachment { get; set; }
    public EmailType EmailType { get; set; }
}