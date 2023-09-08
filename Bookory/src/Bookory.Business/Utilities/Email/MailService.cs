using Bookory.Business.Utilities.DTOs.MailDtos;
using Bookory.Business.Utilities.Email.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Bookory.Business.Utilities.Email;
public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> options)
    {
        _mailSettings = options.Value;
    }
    public async Task SendEmailAsync(MailRequestDto mailRequesDto)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequesDto.ToEmail));
        email.Subject = mailRequesDto.Subject;

        var builder = new BodyBuilder();

        if (mailRequesDto.Attachments != null)
        {
            byte[] fileBytes;
            foreach (var file in mailRequesDto.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }

        builder.HtmlBody = mailRequesDto.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        //smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true; // Bypass certificate validation

        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
