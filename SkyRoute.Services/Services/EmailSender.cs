using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SkyRoute.Domains.Models;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        // DI gebruikt deze constructor
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (_emailSettings == null || _emailSettings.Sender == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            else
            {


                using (var mail = new MailMessage())
                {
                    mail.To.Add(new MailAddress(email));
                    mail.From = new MailAddress(_emailSettings.Sender);
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    try
                    {
                        await SmtpMailAsync(mail);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
        private async Task SmtpMailAsync(MailMessage mail)
        {
            using (var smtp = new SmtpClient(_emailSettings.MailServer))
            {
                smtp.Port = _emailSettings.MailPort;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                smtp.UseDefaultCredentials = false;

                await smtp.SendMailAsync(mail);
            }
        }
    }
}
