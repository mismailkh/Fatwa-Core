using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace FATWA_DOMAIN.Common.Service
{
    public interface IEmailService
    {
        void Send(EmailConfiguration emailSetting);
    }
    public class EmailService : IEmailService
    {
        public void Send(EmailConfiguration emailSetting)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(emailSetting.FromEmail));
                email.To.Add(MailboxAddress.Parse(emailSetting.ToEmail));
                email.Subject = emailSetting.EmailSubject;
                if (emailSetting.BodyType == (int)EmailBodyTypeEnum.Html)
                    email.Body = new TextPart(TextFormat.Html) { Text = emailSetting.EmailBody };
                else 
                    email.Body = new TextPart(TextFormat.Text) { Text = emailSetting.EmailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(emailSetting.SmtpHost, int.Parse(emailSetting.SmtpPort), SecureSocketOptions.Auto);
                smtp.Authenticate(emailSetting.SmtpUser, emailSetting.SmtpPass);
                smtp.Send(email);
                smtp.Disconnect(true);
            } catch (Exception ex) { 
                Console.WriteLine(ex.Message); 
            }
        }
    }
}
