using System.Net.Mail;
using System.Text;
using ConsoleAppScaffold.Settings;
using Microsoft.Extensions.Logging;

namespace ConsoleAppScaffold;

public class EmailClient
{
    private readonly ILogger<EmailClient> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailClient(ILogger<EmailClient> logger, SmtpSettings smtpSetting)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _smtpSettings = smtpSetting ??
                        throw new ArgumentNullException(nameof(smtpSetting));
    }

    public async Task SendEmailAsync(List<string> toAddress, string subject, string body, bool isBodyHtml = false,
        string attachmentPath = "")
    {
        _logger.LogInformation("Sending email with subject '{Subject}' to {ToAddress}", subject, string.Join(", ", toAddress));
        try
        {
            using var smtp = new SmtpClient();
            smtp.Host = _smtpSettings.Server;
            smtp.Port = _smtpSettings.Port;

            var message = new MailMessage();
            message.From = new MailAddress(_smtpSettings.FromAddress);
            foreach (var address in toAddress) message.To.Add(new MailAddress(address));
            foreach (var address in _smtpSettings.CcAddresses) message.CC.Add(new MailAddress(address));
            foreach (var address in _smtpSettings.BccAddresses) message.Bcc.Add(new MailAddress(address));
            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = isBodyHtml;

            if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
            {
                using var attachment = new Attachment(attachmentPath);
                message.Attachments.Add(attachment);
                await smtp.SendMailAsync(message);
            }
            else
            {
                await smtp.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {ToAddress}", toAddress);
        }
    }
}