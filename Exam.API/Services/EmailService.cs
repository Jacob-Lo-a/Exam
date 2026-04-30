using Exam.Core.interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Exam.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly Emailsettings _settings;

        public EmailService(IOptions<Emailsettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendAsync()
        {
            var message = new MailMessage();
            message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
            message.To.Add(_settings.Username);
            message.Subject = "訂單已成立";
            message.Body = "<h1>這是一封測試文件</h1>";
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(_settings.SmtpServer, _settings.Port))
            {
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
                client.EnableSsl = true; 

                try
                {
                    client.Send(message); 
                    Console.WriteLine("郵件發送成功！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("發送失敗: " + ex.Message);
                }
            }
        }
    }

}
