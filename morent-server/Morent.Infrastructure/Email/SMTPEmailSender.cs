using System.Net.Mail;
using Morent.Infrastructure.Settings;

using IEmailSender = Morent.Core.Interfaces.IEmailSender;
using Microsoft.Extensions.Options;

namespace Morent.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
  private readonly MailServerSettings _mailServerSettings;
  private readonly ILogger<SmtpEmailSender> _logger;

  public SmtpEmailSender(IOptions<MailServerSettings> mailServerSettings, ILogger<SmtpEmailSender> logger)
  {
    _mailServerSettings = mailServerSettings.Value;
    _logger = logger;
  }

  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    var emailClient = new SmtpClient(_mailServerSettings.Hostname, _mailServerSettings.Port);

    var message = new MailMessage
    {
      From = new MailAddress(from),
      Subject = subject,
      Body = body
    };
    message.To.Add(new MailAddress(to));
    await emailClient.SendMailAsync(message);
    _logger.LogWarning("Sending email to {to} from {from} with subject {subject} using {type}.", to, from, subject, this.ToString());
  }
}
